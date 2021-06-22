using System;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;


namespace Karia.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute:Attribute,IAsyncActionFilter
    {
        private const string apiKeyOnHeaderKeyName = "x-api-key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            if (!context.HttpContext.Request.Headers.TryGetValue(apiKeyOnHeaderKeyName, out var apiKeyValue))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var service = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = service.GetValue<string>("Api-Key");
            if (apiKey.Equals(apiKeyValue))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            await next();
        }
    }
}