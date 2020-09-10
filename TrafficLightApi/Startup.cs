using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataAccess.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrafficLights.WorkerService;
using TraficLightsRazorPages.Core.Hubs;
using TraficLightsRazorPages.Core.Workers;
using TraficLightsRazorPages.Data;
using TraficLightsRazorPages.Models;

namespace TrafficLightApi
{
    
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";//added this
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddSignalR();
            services.AddDbContext<TraficLightsContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Transient);
            //   services.AddSingleton<TrafficLight>();
            //services.AddTransient<TrafficLight>();
            services.AddScoped<TrafficLight>();
            services.AddScoped<TrafficLightRepository>();
       //     services.AddScoped<TraficLightsWorker>();
            services.AddMediatR(Assembly.GetExecutingAssembly(), Assembly.Load(("TraficLightsRazorPages.Core")));
            /*services.AddSingleton<IHostedService, Worker>();*/
            services.AddSingleton<TrafficLightsService>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {                                      
                                      builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials();
                                  });
            });//added this

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddMvc();

            services.AddHostedService<Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(MyAllowSpecificOrigins);//added this
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=TrafficLights}/{action=Index}/{id?}");
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<TraficLightsHub>("/lighthub");
                });
            });
        }
    }
}
