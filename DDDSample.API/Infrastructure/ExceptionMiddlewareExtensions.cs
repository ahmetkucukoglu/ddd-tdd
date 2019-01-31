namespace DDDSample.API.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Newtonsoft.Json;
    using System.Net;

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var requestIdFeature = context.Features.Get<IHttpRequestIdentifierFeature>();

                        var errorDetail = new
                        {
                            context.Response.StatusCode,
                            contextFeature.Error.Message,
                            requestIdFeature?.TraceIdentifier
                        };

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetail));
                    }
                });
            });
        }
    }
}
