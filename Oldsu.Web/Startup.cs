using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using Oldsu.DatabaseServices;
using Oldsu.DatabaseServices.MySql;
using Oldsu.Logging;
using Oldsu.Logging.Strategies;
using Oldsu.Web.Authentication;

namespace Oldsu.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            #if DEBUG
                services.AddRazorPages().AddRazorRuntimeCompilation();
            #else
                services.AddRazorPages();
            #endif

            services.AddScoped<AuthenticationService>();
            services.AddTransient<IBeatmapService, MySqlBeatmapService>();

#if DEBUG
            services.AddScoped(_ =>
                new LoggingManager(new NoLog()));
#else
            services.AddScoped(_ =>
                new LoggingManager(
                    new MongoDbWriter(Environment.GetEnvironmentVariable("OLDSU_MONGO_DB_CONNECTION_STRING")!)));
#endif
            
            
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.GetFullPath("../html/css")),
                HttpsCompression = HttpsCompressionMode.Compress,
                RequestPath = "/resources/css"
            });
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.GetFullPath("../html/image")),
                HttpsCompression = HttpsCompressionMode.Compress,
                RequestPath = "/resources/image"
            });
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.GetFullPath("../html/js")),
                HttpsCompression = HttpsCompressionMode.Compress,
                RequestPath = "/resources/js"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.GetFullPath(FolderConfiguration.AvatarsFolder)),
                HttpsCompression = HttpsCompressionMode.Compress,
                RequestPath = "/avatars"
            });
            
            app.UseRouting(); 
            
            app.UseMiddleware<AuthenticationMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}