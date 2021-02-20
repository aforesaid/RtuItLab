namespace Identity.API.Models
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(ApplicationUser user, string token)
        {
            Id       = user.Id;
            UserName = user.UserName;
            Token    = token;
        }
    }
}