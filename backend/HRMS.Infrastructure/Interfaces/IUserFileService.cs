using HRMS.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces
{
    public interface IUserFileService
    {
        Task<Dictionary<UserFileCategory, string>> ProcessUploadRequestAsync(IList<UserFileUploadRequest> uploadRequestList);
        string GetImageFileContent(string fileName);

    }
}
