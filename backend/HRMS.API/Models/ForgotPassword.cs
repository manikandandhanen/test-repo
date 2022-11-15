using System.ComponentModel.DataAnnotations;

namespace HRMS.API.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
