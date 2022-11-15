using Microsoft.AspNetCore.Http;

namespace HRMS.Infrastructure.DTOs
{
    public class UserFileUploadRequest
    {
        public string UserId { get; set; }
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        public UserFileCategory UserFileCategory { get; set; }
    }

    public class UserFileUploadResponse
    {
        public bool Suceeded { get; set; }
        public string ErrorMessage { get; set; }
        public string FilePath { get; set; }
    }

    public enum UserFileCategory
    {
        UserProfilePicture,
        UserAsdhaarFile,
        UserPANFile,

    }
}
