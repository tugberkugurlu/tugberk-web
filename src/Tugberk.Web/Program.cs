using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Tugberk.Web
{
    public class Program
    {   
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    // see https://github.com/aspnet/MetaPackages/blob/2.1.4/src/Microsoft.AspNetCore/WebHost.cs#L150
                    var env = hostingContext.HostingEnvironment;
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables("TUGBERKWEB_");

                    if (args != null)
                    {
                        builder.AddCommandLine(args);
                    }
                })
                .UseStartup<Startup>()
                .Build();
    }
}
