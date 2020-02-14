using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Santa.Logic.Interfaces;
using Santa.Data.Entities;
using Santa.Data.Repository;
using Microsoft.OpenApi.Models;

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
            string connectionString = Configuration.GetConnectionString("SantaBaseAppDb");


            services.AddDbContext<Santa.Data.Entities.SantaBaseContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IRepository, Repository>();

            services.AddControllers();
            services.AddHttpClient();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SantaPone API", Version = "v1" });
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            });
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

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SantaPone Central V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
