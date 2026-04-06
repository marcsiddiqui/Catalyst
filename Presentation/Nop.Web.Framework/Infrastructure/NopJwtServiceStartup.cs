using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using Nop.Core.Infrastructure;
using Nop.Web.Framework.Infrastructure.Extensions;
using Nop.Web.Framework.Middleware;
using Nop.Web.Framework.Mvc.Routing;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Nop.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring JWT and Swagger on Start Up
    /// </summary>
    public class NopJwtServiceStartup : INopStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                foreach (var API_Group in SwaggerAPIGroupCredential.Groups)
                    swagger.SwaggerDoc(API_Group.Name.ToLower(), new OpenApiInfo
                    {
                        Version = API_Group.Name,
                        Title = $"{API_Group.Name} Web API",
                        Description = $"{API_Group.Name} Web API"
                    });

                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUz\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });

                // using System.Reflection;
                var xmlFilename = $"Nop.Web.xml";
                swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
            services.AddTokenAuthentication(configuration);

            // configure strongly typed settings object
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseSwaggerAuthorized();
            application.UseSwagger();
            application.UseSwaggerUI(c =>
            {
                foreach (var API_Group in SwaggerAPIGroupCredential.Groups)
                    c.SwaggerEndpoint($"{API_Group.Name.ToLower()}/swagger.json", $"Catalyst {API_Group.Name} V1");
            });

            // global cors policy
            application.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //application.UseDeveloperExceptionPage();

            application.UseStatusCodePages(async context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized
                // you may also check requests path to do this only for specific methods       
                 && !request.Path.Value.StartsWith("/api"))
                {
                    response.Redirect("/login");
                }
            });

            // custom jwt auth middleware
            application.UseMiddleware<JwtMiddleware>();


        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 1; //error handlers should be loaded first
    }
}