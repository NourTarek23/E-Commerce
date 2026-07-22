using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes;

public class RedisCacheAttribute(int timeInSecs) : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Get Cache from DI
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
        //Check if the cached data exists
        var CacheKey = CreateCacheKey(context.HttpContext.Request);
        
        var value = await cacheService.GetAsync(CacheKey);

        if (!string.IsNullOrEmpty(value))
        {
            //if yes, return the cached data
            context.Result = new ContentResult()
            {
                Content = value,
                StatusCode = 200,
                ContentType = "application/json"
            };
        }
        else
        {
            //else , Execute the action and cache the result
            var executed = await next.Invoke();
            if (executed.Result is OkObjectResult okResult)
            {
                await cacheService.SetAsync(CacheKey, okResult.Value, TimeSpan.FromSeconds(timeInSecs));
            }
        }

    }


    private static string CreateCacheKey(HttpRequest request)
    {
        var key = new StringBuilder();
        key.Append(request.Path);

        foreach (var item in request.Query)
        {
            key.Append($"{item.Key} | {item.Value}");
        }

        return key.ToString();
    }
}
