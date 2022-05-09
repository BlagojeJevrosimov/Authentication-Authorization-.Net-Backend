using Microsoft.IdentityModel.Tokens;
using Praksa.BLL.Contracts.Helpers;
using Praksa.BLL.Contracts.Services;
using Praksa.BLL.DTOs.UserController;
using Praksa.BLL.Exceptions;
using Praksa.BLL.Services;
using Praksa.DAL.Contracts.Repositories;
using Praksa.DAL.Entities;
using Praksa.DAL.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Praksa.Services
{

    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashHelper _hashHelper;
        private readonly IEncryptionHelper _encryptionHelper;


        public UserService(IUserRepository userRepository, IHashHelper hashHelper, IEncryptionHelper encryptionHelper) : base(userRepository)
        {
            _userRepository = userRepository;
            _hashHelper = hashHelper;
            _encryptionHelper = encryptionHelper;
        }


        public async Task DeleteUserAsync(int id)
        {
            User user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new BusinessException("User with given username doesn't exist", System.Net.HttpStatusCode.BadRequest);

            await DeleteAsync(id);
        }

        public async Task<User> GetUserByIdAsync(int userId, Role userRole, int id)
        {
            if (userRole == Role.Basic && userId != id)
                throw new BusinessException("Unauthorized request.", System.Net.HttpStatusCode.Unauthorized);

            User user = await GetByIdAsync(id);

            if (user is null)
                throw new BusinessException("User with given id doesn't exist.", System.Net.HttpStatusCode.BadRequest);

            if (userRole == Role.Basic)
            {
                user.Password = "";
                user.SecretId = "";
            }

            return user;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            User user = await _userRepository.GetByUsernameAsync(request.Username!);

            if (user is null)
                throw new BusinessException("User with given username doesn't exists", System.Net.HttpStatusCode.BadRequest);

            string hashedPassword = _hashHelper.Hash(request.Password!);

            if (_hashHelper.CompareHashCodes(user.Password!, hashedPassword))
            {
                string token = generateJwtToken(new TokenRequestDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserRole = user.UserRole
                });

                return new LoginResponseDTO(token);
            }
            else
                throw new BusinessException("Invalid password.", System.Net.HttpStatusCode.BadRequest);
        }

        public string RefreshToken(int userId, string userName, Role userRole)
        {
            string token = generateJwtToken(new TokenRequestDTO()
            {
                Id = userId,
                UserName = userName,
                UserRole = userRole
            });

            return token;
        }

        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO request)
        {
            User foundUser = await _userRepository.GetByUsernameAsync(request.UserName!);

            if (foundUser is null)
            {
                User user = new User()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    Email = request.Email,
                    UserRole = request.UserRole
                };

                user.Password = _hashHelper.Hash(request.Password!);

                bool done = false;

                while (!done)
                {
                    try
                    {
                        user.SecretId = Convert.ToBase64String(_encryptionHelper.GenerateKeyDES());

                        user.Id = await _userRepository.AddAsync(user);

                        done = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string token = generateJwtToken(new TokenRequestDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserRole = user.UserRole
                });

                return new RegisterUserResponseDTO()
                {
                    Token = token
                };
            }
            else
                throw new BusinessException("User with given username already exists.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task UpdateUserAsync(int userId, Role userRole, UpdateUserRequestDTO request)
        {
            User oldUser = await _userRepository.GetByIdAsync(request.Id);

            if (oldUser is null)
                throw new BusinessException("User with given Id doesn't exsist.", System.Net.HttpStatusCode.BadRequest);

            User user = new User()
            {
                Id = request.Id,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserRole = request.UserRole,
                Email = request.Email
            };

            if (userRole == Role.Basic)
            {
                if (userId != request.Id || request.SecretId != String.Empty || request.Password != String.Empty || request.UserRole != Role.Basic)
                    throw new BusinessException("Unauthorized request", System.Net.HttpStatusCode.Unauthorized);

                user.SecretId = oldUser.SecretId;
                user.UserRole = oldUser.UserRole;
                user.Password = oldUser.Password;
            }
            else
            {
                user.SecretId = request.SecretId;
                user.UserRole = request.UserRole;
                user.Password = request.Password;
            }

            await UpdateAsync(user);
        }

        public async Task<bool> PasswordCheck(int userId, string password)
        {
            User user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                throw new BusinessException("User with given id doesn't exist.", System.Net.HttpStatusCode.BadRequest);

            string hashedPassword = _hashHelper.Hash(password!);

            return _hashHelper.CompareHashCodes(user.Password!, hashedPassword);
        }
        private string generateJwtToken(TokenRequestDTO tokenRequest)
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes("DwLZfnVk394X1a8XReNfmx2XBpySsL7W");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", tokenRequest.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Claims = new Dictionary<string, object>()
                {
                    {"Id", tokenRequest.Id},
                    {"UserName",tokenRequest.UserName! },
                    {"Role",tokenRequest.UserRole}
                }
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
