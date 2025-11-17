using E_Commerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next ,ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
                await HandleNotFoundEndPointAsync(context);
            }
            catch (Exception ex)
            {
               //logger
                logger.LogError(ex, "something went wromg");
                //custom response
               
                var problem = new ProblemDetails()
                {
                    Title = "Erorr while processing HTTP Request",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status=ex switch
                    {
                        NotFoundException=> StatusCodes.Status404NotFound,
                        _=> StatusCodes.Status500InternalServerError
                    }
                };
                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);

            }
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var problem = new ProblemDetails()
                {
                    Title = "Error will processing the HTTP request - EndPoint not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"EndPoint {context.Request.Path} was not found on the server.",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
