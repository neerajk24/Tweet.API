using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Tweet.API.Interface;
using Tweet.API.Model;
using BCrypt; // Import the BCrypt namespace
using BCrypt.Net; // Import the BCrypt.Net namespace
using Tweet.API.Entities;

namespace Tweet.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SignupRequestModel createUserRequest)
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password),
            };
            await _userRepository.CreateUserAsync(newUser);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Retrieve user by email
                var user = await _userRepository.GetUserByEmail(model.Email);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Verify password
                if (BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // Password matches, user is authenticated
                    // You can generate and return a JWT token for authentication, or set a session, etc.
                    // Example:
                    var token = GenerateJwtToken(user); // Replace with your JWT token generation logic
                    return Ok(new { token });
                }
                else
                {
                    // Password doesn't match, return unauthorized
                    return Unauthorized("Invalid password");
                }
            }
            catch (Exception ex)
            {
                // Handle exception and return error response
                return StatusCode(500, "An error occurred during login");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserInfo(int id)
        {
            // Fetch user information from the repository or database
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            // Create UserInfo object with required user information
            var userInfo = new User
            {
                Id = user.Id,
                Name = user.Name,
                ProfilePicture = user.ProfilePicture,
                Bio = user.Bio,
                FollowersCount = user.FollowersCount, // Assuming you have a Followers collection in your User model
                Tweets = user.Tweets // Assuming you have a Tweets collection in your User model
            };

            return Ok(userInfo);
        }


        //private string GenerateJwtToken(User user)
        private string GenerateJwtToken(User user)
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var secretKey = Encoding.ASCII.GetBytes(jwtConfig.GetValue<string>("SecretKey"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
                    // Add additional claims as needed
                }),
                Expires = DateTime.UtcNow.AddHours(jwtConfig.GetValue<int>("AccessTokenExpirationHours")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
