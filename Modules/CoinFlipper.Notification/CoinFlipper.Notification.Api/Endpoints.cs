using CoinFlipper.Notification.Application.Commands.Email;
using CoinFlipper.Notification.Application.Commands.Email.Handlers;
using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application.Commands;
using CoinFlipper.ServiceDefaults.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.Notification.Api;

public static class Endpoints
{
    private const string BasePath = "notification";
    
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.MapEmailEndpoints();
        
        return app;
    }
    
    private static WebApplication MapEmailEndpoints(this WebApplication app)
    {
        app.MapPost($"/{BasePath}/email/feedback", async (ICommandDispatcher commandDispatcher, 
            string displayName, string email, string content) =>
        {
            var command = new SendFeedbackEmailRequest(displayName, email, content);
            await commandDispatcher.SendAsync<SendFeedbackEmailRequest>(command);
            return Results.Ok();
        });
        
        app.MapPost($"/{BasePath}/email/verification", async (ICommandDispatcher commandDispatcher, 
            string displayName, string email) =>
        {
            var command = new SendVerificationEmailRequest(displayName, email);
            await commandDispatcher.SendAsync<SendVerificationEmailRequest>(command);
            return Results.Ok();
        });
        
        return app;
    }
}