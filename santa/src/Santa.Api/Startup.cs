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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Santa.Logic.Objects;

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
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Bearer authentication scheme with JWT, e.g. \"Bearer eyJhbGciOiJIUzI1NiJ9.e30\"",
                    Name = "Authorization"
                });
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            });

            //Auth
            string domain = $"https://memelydev.auth0.com/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://memelydev.auth0.com/";
                options.Audience = "https://dev-santaponecentral-api.azurewebsites.net/api/";
            });

            services.AddAuthorization(options =>
            {

                //Policy for clients
                options.AddPolicy("create:clients", policy => policy.RequireClaim("permissions", "create:clients"));
                options.AddPolicy("read:clients", policy => policy.RequireClaim("permissions", "read:clients"));
                options.AddPolicy("update:clients", policy => policy.RequireClaim("permissions", "update:clients"));
                options.AddPolicy("delete:clients", policy => policy.RequireClaim("permissions", "delete:clients"));

                //Policy for events
                options.AddPolicy("create:events", policy => policy.RequireClaim("permissions", "create:events"));
                options.AddPolicy("read:events", policy => policy.RequireClaim("permissions", "read:events"));
                options.AddPolicy("update:events", policy => policy.RequireClaim("permissions", "update:events"));
                options.AddPolicy("delete:events", policy => policy.RequireClaim("permissions", "delete:events"));

                //Policy for statuses
                options.AddPolicy("create:statuses", policy => policy.RequireClaim("permissions", "create:statuses"));
                options.AddPolicy("read:statuses", policy => policy.RequireClaim("permissions", "read:statuses"));
                options.AddPolicy("update:statuses", policy => policy.RequireClaim("permissions", "update:statuses"));
                options.AddPolicy("delete:statuses", policy => policy.RequireClaim("permissions", "delete:statuses"));

                //Policy for questions
                options.AddPolicy("create:surveys", policy => policy.RequireClaim("permissions", "create:surveys"));
                options.AddPolicy("read:surveys", policy => policy.RequireClaim("permissions", "read:surveys"));
                options.AddPolicy("update:surveys", policy => policy.RequireClaim("permissions", "update:surveys"));
                options.AddPolicy("delete:surveys", policy => policy.RequireClaim("permissions", "delete:surveys"));

                //Policy for responses
                options.AddPolicy("create:responses", policy => policy.RequireClaim("permissions", "create:responses"));
                options.AddPolicy("read:responses", policy => policy.RequireClaim("permissions", "read:responses"));
                options.AddPolicy("update:responses", policy => policy.RequireClaim("permissions", "update:responses"));
                options.AddPolicy("delete:responses", policy => policy.RequireClaim("permissions", "delete:responses"));

                //Policy for tags
                options.AddPolicy("create:tags", policy => policy.RequireClaim("permissions", "create:tags"));
                options.AddPolicy("read:tags", policy => policy.RequireClaim("permissions", "read:tags"));
                options.AddPolicy("update:tags", policy => policy.RequireClaim("permissions", "update:tags"));
                options.AddPolicy("delete:tags", policy => policy.RequireClaim("permissions", "delete:tags"));

            });

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
