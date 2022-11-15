using System.ComponentModel.DataAnnotations;

namespace HRMS.API.Models
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
    }

    //public class AuthenticatedResponse
    //{
    //    public string Token { get; set; }
    //}
}
