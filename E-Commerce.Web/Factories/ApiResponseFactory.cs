using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var Errors = actionContext.ModelState.Where(E => E.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
            var problem = new ProblemDetails()
            {
                Title = "validation errors",
                Detail = "one or more validation errors occured",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { { "Errors", Errors } }
            };
            return new BadRequestObjectResult(problem);
        }
    }
}
