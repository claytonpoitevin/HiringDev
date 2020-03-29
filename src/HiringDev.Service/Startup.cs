using System.Threading.Tasks;
using HiringDev.Service.External;
using HiringDev.Service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
