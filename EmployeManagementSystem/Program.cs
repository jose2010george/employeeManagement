using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployManagementSystem.Data.Context; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace EmployeManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try 
            { 
                    var host = CreateHostBuilder(args).Build();

                    using (var scope = host.Services.CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        try
                        {
                            var context = services.GetRequiredService<EmployeManagementDbContext>();
                            DbInitializer.Initialize(context);
                        }
                        catch (Exception ex)
                        { 
                            logger.Error(ex, "An error occurred while seeding the database.");
                            throw; 
                        }
                    }

                    host.Run();
         
                    }  
                    catch (Exception ex)  
                    {  
                        logger.Error(ex, "Error in init");  
                        throw;  
                    }  
                    finally  
                    {  
                        NLog.LogManager.Shutdown();  
                    }  
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                                .ConfigureLogging(logging =>
                                {
                                    logging.ClearProviders();
                                    logging.SetMinimumLevel(LogLevel.Information);
                                })
                                .UseNLog(); 
                });
    }


    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EmployeManagementDbContext>
    {
        public EmployeManagementDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<EmployeManagementDbContext>();
            var connectionString = configuration.GetConnectionString("sqlConnection");
            builder.UseSqlServer(connectionString);
            return new EmployeManagementDbContext(builder.Options);
        }
    }

}
