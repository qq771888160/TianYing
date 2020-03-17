using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TianYing.Infrasturcture;
using TianYing.Infrasturcture.Databases;

namespace TianYing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope =host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var dbContext = scope.ServiceProvider.GetService<MyContext>();                  

                   // dbContext.Database.EnsureDeleted();

                    MyContextSeed.SeedAsync(dbContext, loggerFactory).Wait();
                    // dbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    // var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "Database Migration Error!");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
