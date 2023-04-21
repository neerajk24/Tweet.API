using Microsoft.AspNetCore.Mvc;
using Tweet.API.Interface;
using Tweet.API.Model;

namespace Tweet.API.Controller
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestModel createUserRequest)
        {
            // validate input
            if (!base.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check if user with email already exists
            var existingUser = await _userRepository.GetByEmailAsync(createUserRequest.Email);
            if (existingUser != null)
            {
                return Conflict("User with email already exists");
            }

            // create new user
            var newUser = new User
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                Password = createUserRequest.Password
            };
            await _userRepository.CreateUserAsync(newUser);

            return Ok("User registered successfully");
        }

    }
}
