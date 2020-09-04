using System.Reflection;
using DataAccess.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TraficLightsRazorPages.Core.Hubs;
using TraficLightsRazorPages.Core.Workers;
using TraficLightsRazorPages.Data;
using TraficLightsRazorPages.Models;

namespace TraficLightsRazorPages
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
            services.AddSignalR();
            services.AddControllersWithViews();        

            services.AddDbContext<TraficLightsContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
                  ServiceLifetime.Transient);      
            
            services.AddTransient<TrafficLight>();
            services.AddScoped<TrafficLightRepository>();
            services.AddScoped<TraficLightsWorker>();
            services.AddMediatR(Assembly.GetExecutingAssembly(), Assembly.Load(("TraficLightsRazorPages.Core")));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

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
