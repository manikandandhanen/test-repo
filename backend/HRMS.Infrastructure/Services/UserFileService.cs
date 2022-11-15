using HRMS.Infrastructure.DTOs;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Utilities;
using HRMS.Utilities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class UserFileService: IUserFileService
    {
        private readonly IWebHostEnvironment _environment;

        public UserFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }   
        
        public string GetImageFileContent(string fileName)
        {
            string path = PathHelper.Combine(_environment.ContentRootPath, fileName);
            byte[] b = System.IO.File.ReadAllBytes(path);
            return "data:image/png;base64," + Convert.ToBase64String(b);
        }

        public async Task<Dictionary<UserFileCategory, string>> ProcessUploadRequestAsync(IList<UserFileUploadRequest> uploadRequestList)
        {
            var uploadResonseList = new Dictionary<UserFileCategory, string>();

            foreach (var fileRequest in uploadRequestList)
            {
                var uploadResponse = await SaveUserFileAsync(fileRequest);

                if (!uploadResponse.Suceeded)
                {
                    foreach (var filePath in uploadResonseList.Values)
                    {
                       DeleteUserFile(filePath);
                    }

                    throw new Exception(uploadResponse.ErrorMessage);
                }

                uploadResonseList.Add(fileRequest.UserFileCategory, uploadResponse.FilePath);
            }

            return uploadResonseList;
        }

        private async Task<UserFileUploadResponse> SaveUserFileAsync(UserFileUploadRequest fileRequest)
        {
            UserFileUploadResponse uploadResponse;

            try
            {
                var uniqueFileName = FileHelper.GetUniqueFileName(fileRequest.File.FileName);
                var userUploadPath = PathHelper.Combine("users", fileRequest.UserFileCategory.ToString(), fileRequest.UserId, uniqueFileName);
                var fileFullPath = PathHelper.Combine(_environment.ContentRootPath, userUploadPath);

                Directory.CreateDirectory(PathHelper.GetDirectoryName(fileFullPath));

                await fileRequest.File.CopyToAsync(new FileStream(fileFullPath, FileMode.Create));

                uploadResponse = new UserFileUploadResponse { FilePath = userUploadPath, Suceeded = true };

            }
            catch (Exception ex)
            {
                uploadResponse = new UserFileUploadResponse { ErrorMessage = ex.Message };
            }

            return uploadResponse;
        }

        private void DeleteUserFile(string filePath)
        {
            var fileFullPath = PathHelper.Combine(_environment.ContentRootPath, filePath);

            var fileInfo = new FileInfo(fileFullPath);

            if(fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
