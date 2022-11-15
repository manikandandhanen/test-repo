using AutoMapper;
using HRMS.API.Filters;
using HRMS.API.Models;
using HRMS.Entities;
using HRMS.Infrastructure.Constants;
using HRMS.Infrastructure.DTOs;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user/profile")]
    [Authorize]
    public class UserProfileV1Controller : ApiController
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserFileService _userFileService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserProfileV1Controller(
          IUserProfileRepository userProfileRepository,
          IUserFileService userFileService,
            IMapper mapper,
            ILogger logger)
        {
            _userProfileRepository = userProfileRepository;
            _userFileService = userFileService;
            _mapper = mapper;
            _logger = logger;
        }

        //GET: api/v1/user/profile
        [HttpGet] //https://codewithmukesh.com/blog/serilog-in-aspnet-core-3-1/
        [Activities("user profiles List")] // https://www.c-sharpcorner.com/forums/best-ways-to-track-user-activities-in-my-asp-net-core-app
        public async Task<ActionResult> GetAll()
        {
            var userProfiles = await _userProfileRepository.GetAllAsync();

            if (userProfiles == null)
            {
                _logger.Warning($"users unable to get all");

                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UserProfileDto>>(userProfiles));
        }

        //GET: api/v1/user/profile/id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            if (id == null)
            {
                return ModelStateErrorResponseError(nameof(id), $"{nameof(id)} cannot be null or empty");
            }

            var userProfile = await _userProfileRepository.GetByIdAsync(id);

            if (userProfile == null)
            {
                _logger.Warning($"Userprofile unable to find, id: {id}");
                return NotFound();
            }
            userProfile.UserBasicInfo.ProfileImageUrl = @"users/UserProfilePicture/fcfec07e-98d7-4485-947c-fac4ad959c25/MicrosoftTeams-image (1)_78cd.png";

            if (userProfile?.UserBasicInfo?.ProfileImageUrl != null)
            {
                userProfile.UserBasicInfo.ProfileImageUrl = _userFileService.GetImageFileContent(userProfile.UserBasicInfo.ProfileImageUrl);
            }

            return Ok(_mapper.Map<UserProfileDto>(userProfile));
        }

        // GET: api/v1/user/profile/resources
        [HttpGet("resources")]
        public string Get(string fileName)
        {
            return _userFileService.GetImageFileContent(fileName);
            //string path = _hostingEnvironment.WebRootPath + "/images/" + fileName;
            //byte[] b = System.IO.File.ReadAllBytes(path);
            //return "data:image/png;base64," + Convert.ToBase64String(b);
        }

        // POST: api/v1/user/profile
        [HttpPost]
        public async Task<ActionResult> SaveUserProfile([Bind("UserId")][FromBody] UserProfileDto model)
        {
            var userExists = await _userProfileRepository.UserExistsAsync(model.UserId);
            
            if (!userExists)
            {
                _logger.Warning($"User profile creation: UserId '{model.UserId}' not associated with user");

                return IdentityErrorResponseError("User Profile", $"UserId: '{model.UserId}' doesnot exists");
            }

            var exsists = await _userProfileRepository.ExistsAsync<UserProfile>(model.UserId);

            if(exsists)
            {
                _logger.Warning($"User profile creation: UserId '{model.UserId}' already exists");

                return IdentityErrorResponseError("User Profile", $"UserId: '{model.UserId}' already exists");
            }

            var profile = new UserProfile { UserId = model.UserId };

            await _userProfileRepository.AddAsync(profile);
            _logger.Information($"User profile has been created successfully, UserId: {profile.UserId}");

            return Ok(_mapper.Map<UserProfileDto>(profile));
        }

        // DELETE: api/v1/user/profile/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveUserProfile(string id)
        {
            return await RemoveAction<UserProfile>(id);
        }

        // POST: api/v1/user/profile/contactInfo
        [HttpPost("contactInfo")]
        public async Task<ActionResult> SaveUserContactInfo([FromBody] UserContactInfoDto model)
        {      
            return await SaveAction<UserContactInfo, UserContactInfoDto>(model);
        }

        // DELETE: api/v1/user/profile/contactInfo/id
        [HttpDelete("contactInfo/{id}")]
        public async Task<ActionResult> RemoveUserContactInfo(string id)
        {
            return await RemoveAction<UserContactInfo>(id);
        }


            // POST: api/v1/user/profile/basicInfo
        [HttpPost("basicInfo")]
        public async Task<ActionResult> SaveUserBasicInfo([FromForm] UserBasicInfoDto model)
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(nameIdentifier != model.UserId)
            {
               return BadRequest("Invalid user submission");
            }
            //validate file requests
            var uploadRequestList = GetBasicInfoFileUploadRequests(model);
            var uploadResonseList = await _userFileService.ProcessUploadRequestAsync(uploadRequestList);

             model.ProfilePictureUrl = TryGetValue(uploadResonseList, UserFileCategory.UserProfilePicture) ?? model.ProfilePictureUrl;
             model.AadhaarAttachmentUrl = TryGetValue(uploadResonseList, UserFileCategory.UserAsdhaarFile) ?? model.AadhaarAttachmentUrl;
             model.PANAttachmentUrl = TryGetValue(uploadResonseList, UserFileCategory.UserPANFile) ?? model.PANAttachmentUrl;
            //throw new System.Exception();
            return await SaveAction<UserBasicInfo, UserBasicInfoDto>(model);
        }

        // DELETE: api/v1/user/profile/id
        [HttpDelete("basicInfo/{id}")]
        public async Task<ActionResult> RemoveUserBasicInfo(string id)
        {
            return await RemoveAction<UserBasicInfo>(id);
        }

        private IList<UserFileUploadRequest> GetBasicInfoFileUploadRequests(UserBasicInfoDto model)
        {
            var uploadRequestList = new List<UserFileUploadRequest>();

            if(model.ProfilePictureFile != null)
            {
                uploadRequestList.Add(new UserFileUploadRequest
                {
                    File = model.ProfilePictureFile,
                    UserFileCategory = UserFileCategory.UserProfilePicture,
                    UserId = model.UserId
                });
            }

            if (model.AadhaarFile != null)
            {
                uploadRequestList.Add(new UserFileUploadRequest
                {
                    File = model.AadhaarFile,
                    UserFileCategory = UserFileCategory.UserAsdhaarFile,// = "ProfilePicture",
                    UserId = model.UserId
                });
            }

            if (model.PANFile != null)
            {
                uploadRequestList.Add(new UserFileUploadRequest
                {
                    File = model.PANFile,
                    UserFileCategory = UserFileCategory.UserPANFile,
                    UserId = model.UserId
                });
            }

           return uploadRequestList;
        }

        private string TryGetValue(Dictionary<UserFileCategory, string> dict, UserFileCategory key)
        {
            if (dict.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        #region private Actions

        private async Task<ActionResult> SaveAction<TEntity,TEntityDto>(TEntityDto model) where TEntity : IUserProfile, new() where TEntityDto : IUserProfileDto
        {
            var profileExists = await _userProfileRepository.ExistsAsync<UserProfile>(model.UserId);

            var userProfile = new UserProfile
            {
                UserId = model.UserId
            };

            if (!profileExists)
            {
                await _userProfileRepository.AddAsync<UserProfile>(userProfile);
            }

            var modelMappedEntity = _mapper.Map<TEntity>(model);
            var entityExists = await _userProfileRepository.ExistsAsync<TEntity>(model.UserId);

            if (entityExists)
            {
                var orgEntity = await _userProfileRepository.GetByIdAsync<TEntity>(model.UserId);

                var newEntity = _mapper.Map(modelMappedEntity, orgEntity);

                await _userProfileRepository.UpdateAsync(newEntity);

                _logger.Information($"{typeof(TEntity).Name} has been updated successfully, id: {modelMappedEntity.UserId}");

                return Ok(_mapper.Map<TEntityDto>(newEntity));

            }
            else
            {
                _logger.Information($"{typeof(TEntity).Name} has been {(entityExists ? "updated" : "created") } successfully, id: {modelMappedEntity.UserId}");

                await _userProfileRepository.AddAsync(modelMappedEntity);

                return Ok(_mapper.Map<TEntityDto>(modelMappedEntity));
            }
        }

        private async Task<ActionResult> RemoveAction<T>(string id) where T : IUserProfile
        {
            if (id == null)
            {
                return ModelStateErrorResponseError(nameof(id), $"'{nameof(id)}' cannot be null or empty");
            }

            bool exists = await _userProfileRepository.ExistsAsync<T>(id);

            if (!exists)
            {
                var message = $"{typeof(T).Name} unable to find, id: '{id}'";
                _logger.Warning(message);
                return NotFound(message);
            }

            bool deleted = await _userProfileRepository.DeleteAsync<T>(id);
            if (deleted)
            {
                _logger.Information($"{ typeof(T).Name } has been deleted successfully, id: '{id}'");
            }
            else
            {
                _logger.Warning($"{typeof(T).Name} unable to delete, id: '{id}'");
            }

            return Ok(deleted);
        }

        #endregion
    }
}

		
        