using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Intelligent_Editing.Facades;
using Intelligent_Editing.Processors;
using Intelligent_Editing.Configuration;
using Intelligent_Editing.Parsers;

namespace Intelligent_Editing
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
            services.AddScoped<IUploadFacade, UploadFacade>();
            services.AddScoped<IFileProcessor, FileProcessor>();
            services.AddScoped<ILanguageParser, EnglishParser>();

            IConfigurationSection analysisSettings = Configuration.GetSection("AnalysisSettings");
            services.Configure<AnalysisSettings>(analysisSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Upload/Error");

            if(!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Upload/Error");
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
                    pattern: "{controller=Upload}/{action=Index}/{id?}");
            });
        }
    }
}
