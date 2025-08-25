using ProjectCarTest.Data;
using ProjectCarTest.Interfaces;
using ProjectCarTest.Models;
using ProjectCarTest.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;

// Handles database interactions regarding Logging In and Creates a JWT Token for Successful Log In
namespace ProjectCarTest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseService _dbService;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryHour;

        public UserRepository(DatabaseService dbService, IConfiguration configuration)
        {
            _dbService = dbService;

            // JWT Tokenizer Configuration
            // if the default settings is missing, just use the set ones in appsettings.json (bad practice)
            IConfigurationSection jwtSettings = configuration.GetSection("JwtSettings");
            _jwtKey = jwtSettings["SecretKey"] ?? "CarTestClarenceTokenizer1738056870156789013560781";
            _issuer = jwtSettings["Issuer"] ?? "API";
            _audience = jwtSettings["Audience"] ?? "User";
            _expiryHour = int.Parse(jwtSettings["ExpiryInHours"] ?? "3");
        }

        public LoginResponseDto Login(LoginRequestDto request)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"SELECT * FROM Users WHERE username = @Username AND password = @Password";
            var user = connection.QueryFirstOrDefault<User>(sql, new { Username = request.Username, Password = request.Password });

            if (user == null) // if the user username and password don't exist in the database, then issue a fail.
            {
                return new LoginResponseDto
                {
                    Result = "Fail"
                };
            }

            // if login was successful, then create a token that contains the user's info that expires after 3 hours
            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.username,
                CompanyName = user.companyName,
                Result = "True"
            };
        }

        // method only known in the code, creates a token for the user information.
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.userID.ToString()),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("CompanyName", user.companyName)
                }),
                Expires = DateTime.UtcNow.AddHours(_expiryHour),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
