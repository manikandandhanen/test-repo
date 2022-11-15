namespace HRMS.API.Controllers
{
    using AutoMapper;
    using HRMS.API.Models;
    using HRMS.Entities;
    using HRMS.Infrastructure.Constants;
    using HRMS.Interfaces;
    using HRMS.Notification;
    using HRMS.Utilities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ClientAppMetadata _clientAppMetadata;

        private readonly IEmailSender _emailSender;

        private readonly ILogger _logger;

        private readonly IMapper _mapper;

        public UserController(
          UserManager<ApplicationUser> userManager,
          RoleManager<ApplicationRole> roleManager,
          ClientAppMetadata clientAppMetadata,
          IEmailSender emailSender,
          ILogger logger,
          IMapper mapper
          )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _clientAppMetadata = clientAppMetadata;
            _emailSender = emailSender;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var users = _userManager
               .Users
               .Include(u => u.UserRoles)
               .ThenInclude(ur => ur.Role);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            if (id == null)
            {
                return ModelStateErrorResponseError(nameof(id), $"{nameof(id)} cannot be null or empty");
            }

            var user = await GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult> EmailExists([FromQuery] string email)
        {
            if (email == null)
            {
                return ModelStateErrorResponseError(nameof(email), $"{nameof(email)} cannot be null or empty");
            }

            var user = await _userManager.FindByEmailAsync(email);
            return Ok(user != null);
        }

        [HttpPost("invite")]
        public async Task<ActionResult> Invite([Bind("Email, UserRole, RequestProfile")][FromBody] InviteUserDto model)
        {

            var orgUser = await _userManager.FindByEmailAsync(model.Email);

            if (orgUser != null)
            {
                return IdentityErrorResponseError("UserEmail", $"'{model.Email}' has already exists!");
            }

            bool roleExist = await _roleManager.RoleExistsAsync(model.UserRole);

            if (!roleExist)
            {
                _logger.Warning($"User Creation: Role {model.UserRole} does not exist");

                return IdentityErrorResponseError("UserCreation", $"Role {model.UserRole} does not exist");
            }


            var profileStatus = model.RequestProfile.HasValue ? ProfileStatus.Pending : ProfileStatus.None;

            var applicationUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                UserStatus = UserStatus.Invited,
                ProfileStatus = profileStatus                
            };

            IdentityResult createUserResult = await _userManager.CreateAsync(applicationUser);

            if (!createUserResult.Succeeded)
            {
                string message = string.Join(",", createUserResult.Errors.Select(s => s.Description));
                _logger.Warning($"Create user failed, Email:{model.Email}, Errors:{ message }");
                return IdentityErrorResponseError(createUserResult.Errors);
            }

            _logger.Information($"Created user successfully, Email:{model.Email}");


            IdentityResult addRoleResult = await _userManager.AddToRoleAsync(applicationUser, model.UserRole);

            if (!addRoleResult.Succeeded)
            {
                string message = string.Join(",", addRoleResult.Errors.Select(s => s.Code + ": " + s.Description));
                _logger.Information($"Assigning role failed, Role:{string.Join(',', applicationUser.UserRoles)}, Errors:{ message }");

                return IdentityErrorResponseError(addRoleResult.Errors);
            }
            InvitedUserResponse inviteUserResponse = new InvitedUserResponse();
            try
            {
                // Send an email with this link
                string code = await _userManager.GenerateUserTokenAsync(applicationUser, _userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation");

                string callbackUrl = $"{ _clientAppMetadata.RegisterUserClientUrl}/{applicationUser.Id}/?code={HttpUtility.UrlEncode(code)}";


                string currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

                var templatePath = PathHelper.Combine(currentDirectoryPath, "Notification/Mailtemplates/UserInvitation-min.html");

                var userInvitationTemplate = System.IO.File.ReadAllText(templatePath)
                    .Replace("{{ACCOUNT-ACTIVATION-LINK}}", callbackUrl);

                //  var message = "Please confirm your Account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>";

                await _emailSender.SendEmailAsync(applicationUser.Email, "User Invitation", userInvitationTemplate);
                var message = $"User {applicationUser.Email} created a new account and sent link to registration link to activate.";
                _logger.Information(message);

                inviteUserResponse.Message = message;

            }
            catch (System.Net.Sockets.SocketException se)
            {
                var message = $"User {applicationUser.Email} created a new account and unable to send registration link to mail.";
                _logger.Warning($"{message} Exception {se}");

                inviteUserResponse.Message = message;

            }
            catch (Exception ex)
            {
                var message = $"User {applicationUser.Email} created a new account and unable to send registration link to mail.";

                inviteUserResponse.Message = message;

                _logger.Error(ex.ToString());
            }

            inviteUserResponse.IsInviteSuccessful = true;
            var inviteUser = _mapper.Map<UserDto>(applicationUser);
            inviteUserResponse.InvitedUser = inviteUser;
            return Ok(inviteUserResponse);
        }

        [HttpPut("{id}/update/roles")]
        public async Task<ActionResult> UpdateRoles(string id, [FromBody] string[] UserRoles)
        {
            if (id == null)
            {
                return ModelStateErrorResponseError(nameof(id), $"{nameof(id)} cannot be null or empty");
            }

            if (UserRoles == null)
            {
                return ModelStateErrorResponseError(nameof(UserRoles), $"{nameof(UserRoles)} cannot be null or empty");
            }

            var userToUpdate = await GetUserByIdAsync(id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            var oldUserRoles = userToUpdate.UserRoles.Select(s => s.Role.Name);

            var areTheyEqual = oldUserRoles.ToList().SequenceEqual(UserRoles);

            if (!areTheyEqual)
            {
                if (oldUserRoles.Any())
                {
                    var removeRoleResult = await _userManager.RemoveFromRolesAsync(userToUpdate, oldUserRoles);
                    if (!removeRoleResult.Succeeded)
                    {
                        return IdentityErrorResponseError(removeRoleResult.Errors);
                    }
                }
                var addRoleResult = await _userManager.AddToRolesAsync(userToUpdate, UserRoles);

                if (!addRoleResult.Succeeded)
                {
                    return IdentityErrorResponseError(addRoleResult.Errors);
                }
            }
            return Ok(true);
        }

        [HttpPut("update/status")]
        public async Task<ActionResult> UpdateUserstatus(UserStatusInputDto userStatus)
        {
            if (!Enum.TryParse<UserStatus>(userStatus.Status, out UserStatus status))
            {
                return ModelStateErrorResponseError(nameof(userStatus.Status), $"{nameof(userStatus.Status)} invalid user status");
            }

            var userToUpdate = await GetUserByIdAsync(userStatus.UserId);

            if (userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.UserStatus = status;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);

            if (!updateResult.Succeeded)
            {
                return IdentityErrorResponseError(updateResult.Errors);
            }

            return Ok(updateResult.Succeeded);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            if (id == null)
            {
                return ModelStateErrorResponseError(nameof(id), $"{nameof(id)} cannot be null or empty");
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!removeRoleResult.Succeeded)
            {
                return IdentityErrorResponseError(removeRoleResult.Errors);
            }

            IdentityResult deleteUserResult = await _userManager.DeleteAsync(user);

            if (!deleteUserResult.Succeeded)
            {
                return IdentityErrorResponseError(deleteUserResult.Errors);
            }

            return Ok(deleteUserResult.Succeeded);
        }

        private async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = await _userManager
              .Users
               .Include(u => u.UserRoles)
              .ThenInclude(ur => ur.Role)
              .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }
    }
}
