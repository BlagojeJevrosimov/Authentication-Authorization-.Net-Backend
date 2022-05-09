using API;
using Microsoft.AspNetCore.Mvc;
using Praksa.BLL.Contracts.Services;
using Praksa.BLL.DTOs.PlatformCredentialsController;
using Praksa.DAL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Praksa.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformCredentialsController : ControllerBase
    {
        private readonly IPlatformCredentialsService _platformCredentialsService;
        private int _userId
        {
            get => Convert.ToInt32(HttpContext?.Items["Id"]);
        }


        public PlatformCredentialsController(IPlatformCredentialsService platformCredentialsService)
        {
            _platformCredentialsService = platformCredentialsService;
        }


        [HttpGet("{requestUserId}")]
        [UserAuthorization]
        public async Task<ActionResult<IEnumerable<GetAllUserPlatformCredentialsResponseDTO>>> GetPlatformCredentialsByUserIdAll(int requestUserId)
        {
            IEnumerable<PlatformCredentials> platformCredentials = await _platformCredentialsService.GetAllPlatformCredentialsByUserIdAsync(_userId, requestUserId);

            List<GetAllUserPlatformCredentialsResponseDTO> getAllUserPlatformCredentials = new List<GetAllUserPlatformCredentialsResponseDTO>();

            foreach (PlatformCredentials item in platformCredentials)
            {
                getAllUserPlatformCredentials.Add(new GetAllUserPlatformCredentialsResponseDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    UserName = item.UserName
                });
            }

            return Ok(getAllUserPlatformCredentials);
        }

        [HttpPost]
        [Route("getpassword")]
        [UserAuthorization]
        public async Task<ActionResult<IEnumerable<PlatformCredentials>>> GetPasswordByPlatformCredentialsId(GetPlatformCredentialPasswordByIdRequestDTO request)
        {
            GetPlatformCredentialPasswordByIdResponseDTO response = await _platformCredentialsService.GetPlatformCredentialPasswordById(request.PlatformCredentialsId, _userId, request.Password!);

            return Ok(response);
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<ActionResult<int>> Add(PlatformCredentials platformCredentials)
        {
            int result = await _platformCredentialsService.AddAsync(_userId, platformCredentials);

            return Ok(result);
        }

        [HttpPut]
        [UserAuthorization]
        public async Task<ActionResult<int>> Put(PlatformCredentials platformCredentials)
        {
            int result = await _platformCredentialsService.UpdateAsync(_userId, platformCredentials);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [UserAuthorization]
        public async Task<ActionResult> Delete(int id)
        {
            await _platformCredentialsService.DeleteAsync(_userId, id);

            return NoContent();
        }
    }
}
