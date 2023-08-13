﻿using AzureNaming.Tool.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AzureNaming.Tool.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "APIKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided!"
                };
                return;
            }

            var config = ConfigurationHelper.GetConfigurationData();
            if (GeneralHelper.IsNotNull(config))
            {
                if (!GeneralHelper.DecryptString(config.APIKey!, config.SALTKey!).Equals(extractedApiKey))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "Api Key is not valid!"
                    };
                    return;
                }
            }

            await next();
        }
    }
}
