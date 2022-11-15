using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS.API.Models
{

    public class SampleDto
    {
        public IFormFile ProfilePictureFile { get; set; }
        public IFormFile PANFile { get; set; }
        public IFormFile AadhaarFile { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
            
    }
    public class InviteUserDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(254)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        public string UserRole { get; set; }

        [Required]
        public bool? RequestProfile { get; set; }
    }

    public class InvitedUserResponse
    {
        public UserDto InvitedUser { get; set; }
        public bool IsInviteSuccessful { get; set; }
        public string Message { get; set; }
    }


    public class RegisterUser
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserDto :AuditableDto
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(254)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<string> UserRoles { get; set; }

        public string UserStatus { get; set; }
        public string FullName { get; set; }

        public string ProfileStatus { get; set; }
    }

    public class UserStatusInputDto
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Status { get; set; }        
    }
}