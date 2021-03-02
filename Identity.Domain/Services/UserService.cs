using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RtuItLab.Infrastructure.Models.Identity;

namespace Identity.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
        IOptions<AppSettings> appSettings,
            ILogger<UserService> logger)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appSettings   = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await ValidateUser(user,model.Password))
            {
                var token = GenerateJwtToken(user);
                return new AuthenticateResponse(new User{Id = user.Id, Username = user.UserName}, token)
                {
                    Success = true
                };
            }
            return new AuthenticateResponse
            {
                Success = false
            };
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userDto = new User
            {
                Id = user.Id,
                Username = user.UserName
            };
            return userDto;
        }
        public async Task<IdentityResult> CreateUser(AuthenticateRequest model)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Username
            };
            var response = await _userManager.CreateAsync(applicationUser, model.Password);
            return response;
        }

        public async Task<User> GetUserByToken(TokenRequest model)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
                {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
                }, out var validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;
                var userId = jwtToken?.Claims.First(item => item.Type == "id").Value;
                var user = await  GetUserById(userId);
                return user;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return new User();
            }
        }

        private async Task<bool> ValidateUser(ApplicationUser user, string password)
            => await _signInManager.UserManager.CheckPasswordAsync(user, password);

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key          = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("id",user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
