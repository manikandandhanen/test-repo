using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HRMS.API.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult ModelStateErrorResponseError()
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        protected ActionResult ModelStateErrorResponseError(string key, string errorMessage)
        {
            AddErrors(key, errorMessage);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        protected ActionResult IdentityErrorResponseError(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        protected ActionResult IdentityErrorResponseError(string key, string errorMessage)
        {
            var errors = new Dictionary<string, string[]>
            {
                { key, new string[] { errorMessage } }
            };

            return BadRequest(new ValidationProblemDetails(errors));
        }

        //protected ActionResult ModelStateErrorResponseError(IdentityResult result)
        //{
        //    AddErrors(result);
        //    return BadRequest(new ValidationProblemDetails(ModelState));
        //}

        //protected void AddErrors(IEnumerable<IdentityError> errors)
        //{
        //    foreach (var error in errors)
        //    {
        //        ModelState.AddModelError(error.Code , error.Description);
        //    }
        //}

        protected void AddErrors(string key, string errorMessage)
        {
            ModelState.AddModelError(key, errorMessage);
        }
    }
}