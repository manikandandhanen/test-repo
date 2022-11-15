namespace HRMS.API
{
    using AutoMapper;
    using HRMS.API.AutoMappers;
    using HRMS.API.Extensions;
    using HRMS.API.JwtFeatures;
    using HRMS.Data;
    using HRMS.Entities;
    using HRMS.Infrastructure.Repositories;
    using HRMS.Interfaces;
    using HRMS.Notification;
    using HRMS.Services;
    using HRMS.Utilities;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Serilog;
    using System;
    using System.Text;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.ConfigureApiVersion();

            services.ConfigureSqlContext(Configuration);


            // var appAssembly = typeof(AppDbContext).Assembly.GetName().Name;

            //var assemblys = typeof(EmployeeDBContext).Assembly.GetName().Name;

            //services.AddDbContext<EmployeeDBContext>(config =>
            //{
            //    config.UseSqlite(Configuration.GetConnectionString("APPDBConnection"));
            //});

            //// AddIdentity registers the services
            //services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            //{
            //    config.Password.RequiredLength = 4;
            //    config.Password.RequireDigit = false;
            //    config.Password.RequireNonAlphanumeric = false;
            //    config.Password.RequireUppercase = false;
            //})
            //    .AddEntityFrameworkStores<AppDbContext>()
            //    .AddDefaultTokenProviders();

            services.ConfigureEmailSettings(Configuration);


            var clientAppMetadata = Configuration.GetSection("ClientAppMetadata").Get<ClientAppMetadata>();

            if (clientAppMetadata == null)
            {
                throw new ArgumentNullException("ClientAppMetadata");
            }
            services.AddSingleton(clientAppMetadata);

            services.ConfigureLogger();
            services.ConfigureTransient();
            services.AddAutoMapper(typeof(Program));


            services
                .AddIdentity<ApplicationUser, ApplicationRole>(opt =>
                    {
                        opt.Password.RequiredLength = 7;
                        opt.Password.RequireDigit = false;
                        opt.User.RequireUniqueEmail = true;
                    })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var jwtSettings = Configuration.GetSection("JwtSettings");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(jwtSettings.GetSection("securityKey").Value))
                };
            });

            services.AddScoped<JwtHandler>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("EnableCORS");
            // app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseCors(builder => builder
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
