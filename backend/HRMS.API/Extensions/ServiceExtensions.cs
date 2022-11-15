using HRMS.Data;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Repositories;
using HRMS.Infrastructure.Services;
using HRMS.Interfaces;
using HRMS.Notification;
using HRMS.Services;
using HRMS.Utilities;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

        public static void ConfigureApiVersion(this IServiceCollection services) =>
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver"));
            });


        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
             //services.AddDbContext<RepositoryContext>(opts =>
             //    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("CompanyEmployees")));
             services.AddDbContext<AppDbContext>(config =>
             {
                 config.UseSqlite(configuration.GetConnectionString("sqlLiteConnection"));
             });

        public static void ConfigureEmailSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            if (emailSettings == null)
            {
                throw new ArgumentNullException("NotificationMetadata");
            }
            services.AddSingleton(emailSettings);
        }

        public static void ConfigureTransient(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IUserFileService, UserFileService>();
        }

        public static void ConfigureLogger(this IServiceCollection services)
            {
            Log.Logger = new LoggerConfiguration()
              .WriteTo.File(PathHelper.Combine("/HRMS/Logs/", "Test-Log-{Date}.txt"),              
              rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 100000)
              .CreateLogger();

            services.AddSingleton(Log.Logger);
        }

        //public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        //   services.AddScoped<IRepositoryManager, RepositoryManager>();

    }
}
