//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sassa.Brm.Reports.Components;
using razor.Components;
using Sassa.BRM.Models;
using Sassa.Brm.Reports.Services;
//using Sassa.Socpen.Data;
using System.Configuration;

namespace Sassa.Brm.Reports
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
