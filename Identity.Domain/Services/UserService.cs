using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.DAL.ContextModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServicesDtoModels.Models.Identity;

namespace Identity.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSettings _appSettings;
        public UserService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
        IOptions<AppSettings> appSettings)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _appSettings   = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await ValidateUser(user,model.Password))
            {
                var token = GenerateJwtToken(user);
                return new AuthenticateResponse(new User{Id = user.Id, Username = user.UserName}, token);
            }
            //TODO: return "wrong password"
            return null;
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

        private async Task<bool> ValidateUser(ApplicationUser user, string password)
            => await _signInManager.UserManager.CheckPasswordAsync(user, password);

        private string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key          = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] {new Claim("id",user.Id)}),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
