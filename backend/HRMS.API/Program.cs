namespace HRMS.API
{
    using HRMS.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build()
                 .MigrateDatabases()
                 .CreateRolesAndAdminUser()

                 .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
             .ConfigureAppConfiguration(configurationBuilder =>
             {
                 configurationBuilder.AddEnvironmentVariables();
             }).UseSerilog();
    }
}
