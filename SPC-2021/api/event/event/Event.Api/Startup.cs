using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using Event.Data.Entities;
using Event.Logic.Interfaces;
using Event.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Survey.Api.Authorization;

namespace Event.Api
{
    public class Startup
    {
        private const string version = "v2";
        private const string ConnectionStringName = "EventDb";
        readonly string origins = "AllowSpecificOrigin";
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
                options.AddPolicy(name: origins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://www.santaponecentral.net",
                                                          "http://localhost:4200")
                                            .AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .AllowCredentials();
                                  });
            });

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

            // Auth0
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(options =>
            {

                var objects = new[] { "clients", "events", "statuses", "surveys", "responses", "tags", "profile", "messages", "histories", "boardEntries", "entryTypes", "assignmentStatuses", "logs", "categories" };
                var verbs = new[] { "create", "read", "update", "delete" };

                // cartesian product
                var permissions = objects.SelectMany(o => verbs.Select(v => $"{v}:{o}"));
                foreach (string permission in permissions)
                {
                    options.AddPolicy(permission, policy => policy.Requirements.Add(new HasScopeRequirement(permission, domain)));
                }

            });

            services.AddControllers();
            services.AddHttpClient();

            //Services
            services.AddScoped<IRepository, Repository>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(origins);

            app.UseAuthentication();
            app.UseAuthorization();



            //Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + version + "/swagger.json", "SantaPone Central " + version.ToUpper());
            });

            // Ensures DB is created against container
            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            SantaPoneCentralDatabaseContext dbContext = serviceScope.ServiceProvider.GetService<SantaPoneCentralDatabaseContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
