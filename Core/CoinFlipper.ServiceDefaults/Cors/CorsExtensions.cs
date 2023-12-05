using CoinFlipper.ServiceDefaults.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CorsOptions = CoinFlipper.ServiceDefaults.Options.CorsOptions;

namespace CoinFlipper.ServiceDefaults.Cors;

public static class CorsExtensions
{
	private const string SettingsSectionName = "Cors";
	private const string DefaultPolicyName = "Default";

	#region Methods

	public static IHostApplicationBuilder AddCustomCors(this IHostApplicationBuilder builder)
	{
		var corsOptions = builder.Services.GetOptions<CorsOptions>(SettingsSectionName);

		if (corsOptions?.Enabled == true && corsOptions.Policies?.Any() == true)
			builder.Services.AddCors(options =>
			{
				foreach (var policyOptions in corsOptions.Policies)
					if (IsDefaultPolicy(policyOptions))
						options.AddDefaultPolicy(policyBuilder => { ConfigurePolicy(policyBuilder, policyOptions); });
					else
						options.AddPolicy(policyOptions.Name, policyBuilder => { ConfigurePolicy(policyBuilder, policyOptions); });
			});

		return builder;
	}
	
	public static WebApplication UseCustomCors(this WebApplication app)
	{
		var corsOptions = app.Services.GetOptions<CorsOptions>(SettingsSectionName);

		if (corsOptions?.Enabled != true) 
			return app;
		
		foreach (var policy in corsOptions.Policies)
			if (IsDefaultPolicy(policy))
				app.UseCors(); // Uses the default CORS policy
			else
				app.UseCors(policy.Name); // Uses the named CORS policy

		return app;
	}

	#endregion
	
	#region Private Helpers

	private static bool IsDefaultPolicy(CorsPolicyOptions policy)
		=> policy.Name?.Equals(DefaultPolicyName, StringComparison.OrdinalIgnoreCase) == true;

	private static void ConfigurePolicy(CorsPolicyBuilder policyBuilder, CorsPolicyOptions policyOptions)
	{
		if (policyOptions.AllowAnyOrigin == true)
			policyBuilder.SetIsOriginAllowed(_ => true);
		else if (policyOptions.AllowedOrigins?.Any() == true) 
			policyBuilder.WithOrigins(policyOptions.AllowedMethods);

		if (policyOptions.AllowAnyMethod == true)
			policyBuilder.AllowAnyMethod();
		else if (policyOptions.AllowedMethods?.Any() == true) 
			policyBuilder.WithMethods(policyOptions.AllowedMethods);

		if (policyOptions.AllowAnyHeader == true)
			policyBuilder.AllowAnyHeader();
		else if (policyOptions.AllowedHeaders?.Any() == true) 
			policyBuilder.WithHeaders(policyOptions.AllowedHeaders);

		if (policyOptions.AllowCredentials == true) 
			policyBuilder.AllowCredentials();

		if (policyOptions.ExposedHeaders?.Any() == true) 
			policyBuilder.WithExposedHeaders(policyOptions.ExposedHeaders);
	}
	
	#endregion
}