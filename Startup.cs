using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Blazored.Modal;
using NodaTime;
using TrainingLog.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace TrainingLog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string ConnString = Configuration.GetConnectionString("databasemp");
            var SqlConnectionConfiguration = new SqlDataAccess(Configuration.GetConnectionString("database"));
            services.AddSingleton(SqlConnectionConfiguration);
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            services.AddBlazoredModal();
            services.AddSingleton<State, State>();
            services.AddSingleton<LoadFile, LoadFile>();
            services.AddSingleton<CalculationFunctions>();
            services.AddScoped<ActivitiesLogsService>();            
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
