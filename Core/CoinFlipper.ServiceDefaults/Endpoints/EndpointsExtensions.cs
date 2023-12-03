using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Endpoints;

public static class EndpointsExtensions
{
    public static async Task<ValidationResult> ValidateRequestAsync<T>(T request, HttpContext httpContext) where T : class
    {
        var validator = httpContext.RequestServices.GetService<IValidator<T>>();
        if (validator == null) 
            return new ValidationResult(); 
    
        return await validator.ValidateAsync(request);
    }
    
    public static IDictionary<string, string[]> FormatValidationErrors(ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                failureGroup => failureGroup.Key, 
                failureGroup => failureGroup.Select(failure => failure.ErrorMessage).ToArray()
            );
    }
}