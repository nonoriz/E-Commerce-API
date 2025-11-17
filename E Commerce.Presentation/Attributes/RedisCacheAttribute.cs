using E_Commerce.Services_Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Attributes
{
    public class RedisCacheAttribute: ActionFilterAttribute
    {
        private readonly int durationInMin;

        public RedisCacheAttribute(int DurationInMin=5)
        {
            durationInMin = DurationInMin;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //GET CACHE SERVICE FROM DEPENDENCY INJECTION CONTAINER
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            //create cahce key base on request path and query string
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            //Check id cache data exist
            var cacheValue =await cacheService.GetAsync(cacheKey);
            //If exist return data from cache and skip executing of EndPoint
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/Json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            
            //if not exist execute EndPoint and store result in cache if 200 OK response
            var ExecutedContext= await next.Invoke();
            if(ExecutedContext.Result is ObjectResult result)
            {
                await cacheService.SetAsync(cacheKey,result.Value!.ToString()!,TimeSpan.FromMinutes(durationInMin));

            }




        }
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(x => x.Key))
                key.Append($"|{item.Key}={item.Value}");
            return key.ToString();


        }
    }
}
