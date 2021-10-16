using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Oldsu.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configurator.CreateFolders(); // add to properties "AVATAR_FILE_LOCATION": "C:\\Users\\RiderProjects\\oldsu.web\\Oldsu.Web\\avatars" or any directory
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}