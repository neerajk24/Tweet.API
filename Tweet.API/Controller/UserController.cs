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
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Tweet.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITweetRepository _tweetRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, ITweetRepository tweetRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tweetRepository = tweetRepository;
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
                    //return Ok(new { token });
                    return Ok(new { token, user.Id });
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
                FollowersCount = user.FollowersCount,
                Email = user.Email
            };

            // Fetch user's own tweets from the repository or database
            List<Entities.Tweet> ownTweets = await _tweetRepository.GetTweetsByUserIdAsync(id);

            // Create JsonSerializerOptions with ReferenceHandler.Preserve
            

            // Serialize ownTweets to JSON using the JsonSerializerOptions
 

            // Deserialize ownTweetsJson back to List<Entities.Tweet> to resolve circular references


            // Assign the deserializedOwnTweets to the user info object
            userInfo.Tweets = ownTweets;

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // Optional: Makes the output more readable with indentation
            };

            try
            {
                string userInfoJson = JsonSerializer.Serialize(userInfo, options);
                return Ok(userInfoJson);
            }
            catch (JsonException ex)
            {
                // If there's a circular reference, it will be captured in the JsonException
                // Print or log the ex.Message to identify the path of the circular reference
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception
            }


            return Ok(userInfo);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> EditProfile(int id, [FromBody] UserProfileModel userProfileModel)
        {
            // Fetch user information from the repository or database
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            // Update user profile details with the provided data
            user.Name = userProfileModel.Name;
            user.ProfilePicture = userProfileModel.ProfilePicture;
            user.Bio = userProfileModel.Bio;

            // Save changes to the repository or database
            await _userRepository.UpdateAsync(user);

            return Ok(); // Profile details updated successfully
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
