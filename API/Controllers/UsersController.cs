#nullable disable
using Microsoft.AspNetCore.Mvc;
using Praksa.BLL.Contracts.Services;
using Praksa.BLL.DTOs.UserController;
using Praksa.BLL.Exceptions;
using Praksa.DAL.Entities;
using Praksa.DAL.Enums;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private Role _userRole
        {
            get => (Role)Enum.Parse(typeof(Role), HttpContext?.Items["Role"].ToString());
        }
        private int _userId
        {
            get => Convert.ToInt32(HttpContext?.Items["Id"]);
        }
        private string _userName
        {
            get => HttpContext.Items["UserName"].ToString();
        }


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [UserAuthorization(Role.Admin)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [UserAuthorization]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await _userService.GetUserByIdAsync(_userId, _userRole, id);

            return user;
        }

        [HttpPut]
        [UserAuthorization]
        public async Task<IActionResult> PutUser(UpdateUserRequestDTO request)
        {
            await _userService.UpdateUserAsync(_userId, _userRole, request);

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterUserResponseDTO>> RegisterUser(RegisterUserRequestDTO request)
        {
            RegisterUserResponseDTO response = await _userService.RegisterUserAsync(request);
           
            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO request)
        {
            LoginResponseDTO response = await _userService.Login(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [UserAuthorization(Role.Admin)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);

            return NoContent();
        }

        [HttpGet]
        [Route("refreshToken")]
        [UserAuthorization]
        public IActionResult RefreshToken()
        {
            var token = _userService.RefreshToken(_userId, _userName, _userRole);

            return Ok(new RefreshTokenResponseDTO(token));
        }
    }
}
