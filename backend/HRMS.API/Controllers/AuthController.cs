using AutoMapper;
using HRMS.API.JwtFeatures;
using HRMS.API.Models;
using HRMS.Entities;
using HRMS.Interfaces;
using HRMS.Notification;
using HRMS.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ClientAppMetadata _clientAppMetadata;
        private readonly JwtHandler _jwtHandler;

        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AuthController(
          UserManager<ApplicationUser> userManager,
          ClientAppMetadata clientAppMetadata,
          JwtHandler jwtHandler,
          IEmailSender emailSender,
          IMapper mapper,
          ILogger logger
          )
        {
            _userManager = userManager;
            _clientAppMetadata = clientAppMetadata;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInput model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = await _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUser model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                _logger.Warning($"user not found, userId:{model.UserId}");
                return NotFound($"user not found : {model.UserId}");
            }

            if (user.UserStatus != UserStatus.Invited)
            {
                var message = $"register email failed, Email:{model.Email} not invited , may be invitation revoked";
                _logger.Warning(message);
                return IdentityErrorResponseError("User register", message);
            }

            var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, model.Code);

            if (!confirmEmailResult.Succeeded)
            {
                string message = string.Join(",", confirmEmailResult.Errors.Select(s => s.Description));
                _logger.Warning($"confirm email failed, Email:{model.Email}, Errors:{ message }");
                return IdentityErrorResponseError(confirmEmailResult.Errors);
            }

            var passwordResult = await _userManager.AddPasswordAsync(user, model.Password);
            if (!passwordResult.Succeeded)
            {
                string message = string.Join(",", passwordResult.Errors.Select(s => s.Description));
                _logger.Warning($"resgister user failed, Email:{model.Email}, Errors:{ message }");
                return IdentityErrorResponseError(passwordResult.Errors);
            }

            user.FullName = model.FullName;
            user.UserStatus = UserStatus.Active;
            IdentityResult updateUserResult = await _userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                string message = string.Join(",", updateUserResult.Errors.Select(s => s.Description));
                _logger.Warning($"register user failed, Email:{model.Email}, Errors:{ message }");
                return IdentityErrorResponseError(updateUserResult.Errors);
            }

            _logger.Information($"registered user successfully, Email:{model.Email}");

            try
            {
                string currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

                var templatePath = PathHelper.Combine(currentDirectoryPath, "Notification/Mailtemplates/UserConfirmation-min.html");

                //{{USER-PROFILE-LINK}}
                var userCofirmationTemplate = System.IO.File.ReadAllText(templatePath)

                    .Replace("{{USER-FULLNAME}}", user.FullName)
                    .Replace("{{USER-PROFILE-LINK}}", _clientAppMetadata.UserProfileClientUrl)
                    .Replace("{{PORTAL-NAME}}", "HRMStudio")
                    .Replace("{{CONTACT-EMAIL}}", "hrindia@quadgen");

                await _emailSender.SendEmailAsync(user.Email, "HRMStudio Account Confirmation", userCofirmationTemplate);

                _logger.Information($"User {user.Email} registered a account and sent confirmation mail.");
            }
            catch (System.Net.Sockets.SocketException se)
            {
                _logger.Warning($"User {user.Email} registered a account and unable to send confirmation mail. Exception {se}");
            }
            return Ok();
        }

        [HttpPost("confirmemail")]
        public async Task<ActionResult<IdentityResult>> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return ModelStateErrorResponseError("UserIdOrCode", "userid or code cannot be null or empty");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.Warning($"user not found : {userId}");

                return NotFound($"user not found : {userId}");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result;
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return BadRequest();
            }

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);

            string callbackUrl = $"{ _clientAppMetadata.ResetPasswordClientUrl}?userId={user.Id}&code={HttpUtility.UrlEncode(code)}";

            await _emailSender.SendEmailAsync(model.Email, "HRMStudio Reset Password Link",
               $"Dear <b>{user.FullName ?? "User"}</b>,<br /><br />As part of the verification process to set a new password,  Please follow the link by clicking <a href=\"" + callbackUrl + "\">here</a><br /><br />Warm regards,<br />NPM");
            
            _logger.Information($"password reset email has been sent to email: {model.Email}");
            return Ok(new { message = "password reset email has been sent to email" });
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<IdentityResult>> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateErrorResponseError();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                _logger.Information($"Password has been reset successfully, email: {model.Email}");
                return Ok(new { message = $"Password has been reset successfully"});
            }
            else
            {
                _logger.Information($"Password reset failed, email: {model.Email}");
                return IdentityErrorResponseError(result.Errors);
            }
        }

        [HttpGet("VerifyResetPasswordToken")]
        public async Task<IActionResult> VerifyResetPasswordToken(string userId, string code)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest($"User not found : {userId}");
            }

            bool isValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", code);

            return Ok(new { isValid });
        }

        [HttpGet("VerifyRegistrationToken")]
        public async Task<IActionResult> VerifyRegistrationToken(string userId, string code)
        {
            // return Ok(new UserDto{  Email = "email@sample.com", UserId = userId });

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.Warning($"User not found : {userId}");
                return Ok(null);
            }

            bool isValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation", code);

            return Ok(isValid ? _mapper.Map<UserDto>(user) : null);
        }

    }
}
