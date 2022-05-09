using Praksa.BLL.Contracts.Helpers;
using Praksa.BLL.Contracts.Services;
using Praksa.BLL.DTOs.PlatformCredentialsController;
using Praksa.BLL.Exceptions;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Entities;

namespace Praksa.BLL.Services
{
    public class PlatformCredentialsService : BaseService<PlatformCredentials>, IPlatformCredentialsService
    {
        private readonly IPlatformCredentialsRepository _platformCredentialsRepository;
        private readonly IUserService _userService;
        private IEncryptionHelper _encryptionHelper;


        public PlatformCredentialsService(IPlatformCredentialsRepository platformCredentialsRepository, IEncryptionHelper encryptionHelper, IUserService userService) : base(platformCredentialsRepository)
        {
            _platformCredentialsRepository = platformCredentialsRepository;
            _encryptionHelper = encryptionHelper;
            _userService = userService;
        }


        public async Task<IEnumerable<PlatformCredentials>> GetAllPlatformCredentialsByUserIdAsync(int userId, int requestUserId)
        {
            User user = _userService.GetByIdAsync(requestUserId).Result;

            if (user is null)
                throw new BusinessException("User with given id doesn't exist.", System.Net.HttpStatusCode.BadRequest);

            if (userId != requestUserId)
                if (user.UserRole == DAL.Enums.Role.Basic)
                    throw new BusinessException("Unauthorized request.", System.Net.HttpStatusCode.Unauthorized);

            IEnumerable<PlatformCredentials> platformCredentials = await _platformCredentialsRepository.GetAllByUserIdAsync(requestUserId);

            return platformCredentials;
        }

        public async Task<int> AddAsync(int userId, PlatformCredentials platformCredentials)
        {
            User user = await _userService.GetByIdAsync(userId);

            if (user.UserRole == DAL.Enums.Role.Basic)
                if (userId != platformCredentials.UserId)
                    throw new BusinessException("Unauthorized request", System.Net.HttpStatusCode.Unauthorized);


            int platformCredentialsId = await _platformCredentialsRepository.AddAsync(platformCredentials);

            PlatformCredentials addedEntity = await _platformCredentialsRepository.GetByIdAsync(platformCredentialsId);

            user = await _userService.GetByIdAsync(platformCredentials.UserId);

            if (user is null)
                throw new BusinessException("User with given username deosn't exist", System.Net.HttpStatusCode.BadRequest);

            addedEntity.Password = _encryptionHelper.EncryptStringDES(platformCredentials.Password!, user.SecretId!);

            await _platformCredentialsRepository.UpdateAsync(addedEntity);

            return addedEntity.Id;
        }
        public async Task<int> UpdateAsync(int userId, PlatformCredentials platformCredentials)
        {
            User user = await _userService.GetByIdAsync(userId);

            if (user.UserRole == DAL.Enums.Role.Basic)
                if (userId != platformCredentials.UserId)
                    throw new BusinessException("Unauthorized request", System.Net.HttpStatusCode.Unauthorized);

            PlatformCredentials platformCredentialsEntity = await _platformCredentialsRepository.GetByIdAsync(platformCredentials.Id);

            user = await _userService.GetByIdAsync(platformCredentials.UserId);

            if (user is null)
                throw new BusinessException("User with given username deosn't exist", System.Net.HttpStatusCode.BadRequest);

            platformCredentialsEntity.Password = _encryptionHelper.EncryptStringDES(platformCredentials.Password!, user.SecretId!);

            await _platformCredentialsRepository.UpdateAsync(platformCredentialsEntity);

            return platformCredentialsEntity.Id;
        }

        public async Task<GetPlatformCredentialPasswordByIdResponseDTO> GetPlatformCredentialPasswordById(int platformCredentialId, int userId, string password)
        {
            PlatformCredentials result = await GetByIdAsync(platformCredentialId);

            if (result is null)
                new BusinessException("PlatformCredentials with given id doesn't exist", System.Net.HttpStatusCode.BadRequest);

            if (_userService.PasswordCheck(userId, password).Result)
            {
                User user = await _userService.GetByIdAsync(userId);

                string PlatformCredentialPassword = _encryptionHelper.DecryptStringDES(result!.Password!, user.SecretId!);

                return new GetPlatformCredentialPasswordByIdResponseDTO()
                {
                    Password = PlatformCredentialPassword
                };
            }
            throw new BusinessException("Invalid password.", System.Net.HttpStatusCode.Unauthorized);
        }

        public async Task DeleteAsync(int userId, int platformCredentialsId)
        {
            User user = await _userService.GetByIdAsync(userId);

            PlatformCredentials platformCredentials = await _platformCredentialsRepository.GetByIdAsync(platformCredentialsId);

            if (platformCredentials is null)
                throw new BusinessException("PlatformCredentials with given id deosn't exist.", System.Net.HttpStatusCode.BadRequest);

            if (user.UserRole == DAL.Enums.Role.Basic)
                if (userId != platformCredentials!.UserId)
                    throw new BusinessException("Unauthorized request", System.Net.HttpStatusCode.Unauthorized);

            await _platformCredentialsRepository.DeleteByIdAsync(platformCredentialsId);
        }
    }
}
