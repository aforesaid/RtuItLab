namespace ServicesDtoModels.Models.Identity
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id       = user.Id;
            UserName = user.Username;
            Token    = token;
        }

        public AuthenticateResponse()
        {
            
        }
    }
}