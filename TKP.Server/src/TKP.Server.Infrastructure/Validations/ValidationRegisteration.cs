using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using TKP.Server.Application.Features.Auth.Commands.Login;

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

            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                            .AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        }
    }
}
