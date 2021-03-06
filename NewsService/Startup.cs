using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NewsService.Models;
using NewsService.Repository;
using NewsService.Services;
namespace NewsService
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
            services.AddControllers();
            services.AddSingleton(new NewsContext(Configuration));
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, Services.NewsService>();
            //    services.Configure<NewsServiceDatabaseSettings>(
            //Configuration.GetSection(nameof(NewsServiceDatabaseSettings)));

            //    services.AddSingleton<INewsServiceDatabaseSettings>(sp =>
            //        sp.GetRequiredService<IOptions<NewsServiceDatabaseSettings>>().Value);

            //provide options for Database Context to Register Dependencies
            //Register all dependencies here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
