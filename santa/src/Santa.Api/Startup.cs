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
using System.Linq;
using Santa.Api.AuthHelper;
using Santa.Api.SendGrid;
using Santa.Logic.Objects;

namespace Santa.Api
{
    public class Startup
    {
        private const string version = "v7";
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
                    builder.WithOrigins("http://localhost:4200", "https://dev-santaponecentral.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            //Services
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IAuthHelper, AuthHelper.AuthHelper>();
            services.AddScoped<IMailbag, Mailbag>();


            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = "SantaPone API", Version = version });
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Bearer authentication scheme with JWT, e.g. \"Bearer eyJhbGciOiJIUzI1NiJ9.e30\"",
                    Name = "Authorization"
                });
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            });

            //SignalR
            services.AddSignalR();

            //Auth
            string domain = $"https://{Configuration["Auth0API:domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0API:audience"];
            });

            services.AddAuthorization(options =>
            {

                var objects = new[] { "clients", "events", "statuses", "surveys", "responses", "tags", "profile", "messages", "histories" };
                var verbs = new[] { "create", "read", "update", "delete" };

                // cartesian product
                var permissions = objects.SelectMany(o => verbs.Select(v => $"{v}:{o}"));
                foreach (string permission in permissions)
                {
                    options.AddPolicy(permission, policy => policy.RequireClaim("permissions", permission));
                }

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
                c.SwaggerEndpoint("/swagger/"+ version + "/swagger.json", "SantaPone Central " + version.ToUpper());
            });

            //Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
