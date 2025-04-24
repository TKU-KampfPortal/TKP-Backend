using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TKP.Server.Infrastructure.Swagger
{
    internal static class SwaggerRegisteration
    {
        internal static WebApplicationBuilder AddApplicationSwagger(this WebApplicationBuilder builder)
        {

            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TKP Service API",
                    Description = @"All API methods return an object of the generic type <b>ApiResult &lt;T&gt;</b>
                    <br/><br/>{
                      <br/> ""succeeded"": <b>true</b> or <b>false</b>,
                      <br/> ""result"": [the response object if <b>succeeded = true</b>],
                      <br/> ""errors"": [the error(s) if <b>succeeded = false</b>]
                    <br/>}"
                    // TODO: Description, TermsOfService, Contact, License
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer YOUR_TOKEN')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            }
                        }, new string[]{}
                    }
                });
                option.CustomSchemaIds(type => type.ToString());

            });
            return builder;
        }
    }
}
