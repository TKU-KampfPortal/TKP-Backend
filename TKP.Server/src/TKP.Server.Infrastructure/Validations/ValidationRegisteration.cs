using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using TKP.Server.Application.Features;

namespace TKP.Server.Infrastructure.Validations
{
    public static class ValidationRegisteration
    {
        public static void AddApplicationFluentValidation(this WebApplicationBuilder builder)
        {
            // Register FluentValidation
            builder.Services
                .AddFluentValidationAutoValidation()
                ;

            builder.Services.AddValidatorsFromAssemblyContaining<IMarker>();
        }
    }
}
