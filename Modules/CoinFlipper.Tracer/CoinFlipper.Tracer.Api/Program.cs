using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Cors;
using CoinFlipper.ServiceDefaults.Options;
using CoinFlipper.ServiceDefaults.Swagger;
using CoinFlipper.Tracer.Api;
using CoinFlipper.Tracer.Application;
using CoinFlipper.Tracer.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);

var appOptions = builder.Services.GetOptions<AppOptions>("App");
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(appOptions.Port); 
});

builder.AddServiceDefaults()
    .AddApplication()
    .AddInfrastructure();


var app = builder.Build();

app.UseCustomCors()
    .MapEndpoints()
    .UseSwagger()
    .Run();