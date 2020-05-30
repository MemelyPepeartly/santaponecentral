using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Santa.Logic.Interfaces;
using Santa.Data.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Santa.Api.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Santa.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Connection string
            string connectionString = Configuration.GetConnectionString("SantaBaseAppDb");

            //DBContext
            services.AddDbContext<Santa.Data.Entities.SantaPoneCentralDatabaseContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "http://dev-santaponecentral.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            //Repository
            services.AddScoped<IRepository, Repository>();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SantaPone API", Version = "v1" });
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            });

            //Auth
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:clients", policy => policy.Requirements.Add(new HasScopeRequirement("read:clients", domain)));
                options.AddPolicy("read:profile", policy => policy.Requirements.Add(new HasScopeRequirement("read:profile", domain)));

            });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddMvc();



            services.AddControllers();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            //Auth
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseCors("AllowAngular");

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SantaPone Central V1");
            });

            //Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
