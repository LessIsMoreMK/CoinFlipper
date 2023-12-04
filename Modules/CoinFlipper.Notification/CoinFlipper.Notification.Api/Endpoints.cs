using CoinFlipper.Notification.Application.Commands.Email;
using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application.Commands;
using CoinFlipper.ServiceDefaults.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CoinFlipper.Notification.Api;

public static class Endpoints
{
    private const string BasePath = "notification";
    
    internal static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints()
            .MapEmailEndpoints()
            
            ;
        
        return app;
    }
    
    private static WebApplication MapEmailEndpoints(this WebApplication app)
    {
        app.MapPost($"/{BasePath}/email/feedback", async (HttpContext httpContext, ICommandDispatcher commandDispatcher, 
            string displayName, string email, string content) =>
        {
            var command = new SendFeedbackEmailRequest(displayName, email, content);

            var validationResult = await EndpointsExtensions.ValidateRequestAsync(command, httpContext);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(EndpointsExtensions.FormatValidationErrors(validationResult));
            
            await commandDispatcher.SendAsync<SendFeedbackEmailRequest>(command);
            return Results.Ok();
        });
        
        app.MapPost($"/{BasePath}/email/verification", async (HttpContext httpContext, ICommandDispatcher commandDispatcher, 
            string displayName, string email) =>
        {
            var command = new SendVerificationEmailRequest(displayName, email);
            
            var validationResult = await EndpointsExtensions.ValidateRequestAsync(command, httpContext);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(EndpointsExtensions.FormatValidationErrors(validationResult));
            
            await commandDispatcher.SendAsync<SendVerificationEmailRequest>(command);
            return Results.Ok();
        });
        
        return app;
    }
}