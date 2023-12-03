using CoinFlipper.Notification.Api;
using CoinFlipper.Notification.Application;
using CoinFlipper.Notification.Infrastructure;
using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

var appOptions = builder.Services.GetOptions<AppOptions>("App");
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(appOptions.Port); 
});

builder.AddServiceDefaults();

builder.AddApplication();

builder.AddInfrastructure();

var app = builder.Build();


app.MapEndpoints();

app.Run();