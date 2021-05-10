using Message.Data.Entities;
using Message.Data.Repository;
using Message.Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message.Api
{
    public class Startup
    {
        private const string version = "v2";
        private const string ConnectionStringName = "MessageDb";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DB Connection
            services.AddDbContext<SantaPoneCentralDatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(ConnectionStringName)));


            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "https://dev-spc-2021.azurewebsites.net", "https://www.santaponecentral.net", "https://santaponecentral.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            //Services
            services.AddScoped<IRepository, Repository>();

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
                options.Audience = Configuration["Auth0API:startupAudience"];
            });

            services.AddAuthorization(options =>
            {

                var objects = new[] { "clients", "events", "statuses", "surveys", "responses", "tags", "profile", "messages", "histories", "boardEntries", "entryTypes", "assignmentStatuses", "logs", "categories" };
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
                c.SwaggerEndpoint("/swagger/" + version + "/swagger.json", "SantaPone Central " + version.ToUpper());
            });

            //Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Ensures DB is created against container
            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            SantaPoneCentralDatabaseContext dbContext = serviceScope.ServiceProvider.GetService<SantaPoneCentralDatabaseContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
