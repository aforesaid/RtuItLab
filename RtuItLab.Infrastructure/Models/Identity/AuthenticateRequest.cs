using System.ComponentModel.DataAnnotations;

namespace ServicesDtoModels.Models.Identity
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
