using System.ComponentModel.DataAnnotations;

namespace HRMS.API.Models
{
    public class RoleDto
    {
        public string RoleId { get; set; }

        [Required]
        [MinLength(3,ErrorMessage = "The field Role Name must be a minimum length of '3'.")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}