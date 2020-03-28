using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiringDev.Service.External;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace HiringDev.Service
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
            services.AddControllersWithViews();
            services.Configure<Settings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoConnection:Database").Value;
                    options.YouTubeApiKey = Configuration.GetSection("Youtube:ApiKey").Value;                    
                    options.YouTubeAppname = Configuration.GetSection("Youtube:AppName").Value;
                });

            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(Configuration.GetSection("MongoConnection:ConnectionString").Value));

            services.AddTransient<IMongoDbContext, MongoDbContext>();
            services.AddTransient<IYoutubeResultsRepository, YoutubeResultsRepository>();
            services.AddTransient<IYoutubeLookupService, YoutubeLookupService>();
            services.AddTransient<IYoutubeServiceProvider, YoutubeServiceProvider>();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
