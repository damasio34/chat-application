using ChatApplication.API.SwaggerExamples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace ChatApplication.API.Extensions
{
    public static class SwaggerDocumentationConfigExtension
    {
        public static IServiceCollection AddSwaggerDocumentationConfig(this IServiceCollection service)
        {
            return service
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ChatApplication API",
                        Description = "API of ChatApplication"
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.ExampleFilters();
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Insira aqui o token retornado na rota de login no formato \"Bearer {token}\"",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                   Type = ReferenceType.SecurityScheme,
                                   Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
                })
                .AddSwaggerExamplesFromAssemblyOf<LoginInputExample>();
        }
    }
}
