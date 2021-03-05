using Identity.DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RtuItLab.Infrastructure.Models.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RtuItLab.Infrastructure.Exceptions;
using RtuItLab.Infrastructure.MassTransit;

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

        public async Task<ResponseMassTransit<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            var response = new ResponseMassTransit<AuthenticateResponse>();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await ValidateUser(user, model.Password))
                response.Exception = new BadRequestException("Invalid login or password!");
            else
            {
                var token = GenerateJwtToken(user);
                response.Content = new AuthenticateResponse(new User {Id = user.Id, Username = user.UserName},
                    token);
            }
            return response;
        }

        public async Task<ResponseMassTransit<User>> GetUserById(string id)
        {
            var response = new ResponseMassTransit<User>();
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                response.Exception = new BadRequestException("User not found!");
            else
                response.Content = new User
                {
                    Id = user.Id,
                    Username = user.UserName
                };
            return response;
        }
        public async Task<ResponseMassTransit<IdentityResult>> CreateUser(RegisterRequest model)
        {
            var response = new ResponseMassTransit<IdentityResult>();
            var applicationUser = new ApplicationUser()
            {
                UserName = model.Username
            };
            response.Content = await _userManager.CreateAsync(applicationUser, model.Password);
            return response;
        }

        public async Task<ResponseMassTransit<User>> GetUserByToken(TokenRequest model)
        {
            var response = new ResponseMassTransit<User>();
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
                if (user.Exception != null)
                    response.Exception = user.Exception;
                else
                    response.Content = user.Content;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                response.Exception = new BadRequestException("Invalid request token!");
            }

            return response;
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
