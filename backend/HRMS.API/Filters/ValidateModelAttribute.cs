using Microsoft.AspNetCore.Mvc.Filters;

namespace HRMS.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                //actionContext.HttpContext.Response = actionContext.HttpContext.Response..CreateErrorResponse(
                //    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
