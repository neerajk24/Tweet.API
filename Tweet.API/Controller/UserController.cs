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


namespace Tweet.API.Controller
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            // Validate email and password
            if (!ModelState.IsValid)
            {
                // If email or password is not valid, return appropriate error response
                return BadRequest("Invalid email or password.");
            }

            // Check against the user repository or database
            var user = await _userRepository.GetUserByEmail(model.Email);
            if (user == null || !ValidatePassword(model.Password, user.Password))
            {
                // If user not found or password is incorrect, return appropriate error response
                return BadRequest("Invalid email or password.");
            }

            // Generate authentication tokens (e.g. JWT)
            var token = GenerateJwtToken(user);

            // Return success response with authentication token
            return Ok(new { Token = token });
        }

        private bool ValidatePassword(string password, string hashedPassword)
        {
            // Implement password validation logic, e.g. check for required complexity
            // (e.g. at least one capital letter, one small letter, one number, and one symbol)
            // You can use regular expressions, string manipulation, or third-party libraries for this

            // Example validation using regular expressions
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$");
            return regex.IsMatch(password) && BCrypt.Net.BCrypt.Verify(password, hashedPassword);
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
