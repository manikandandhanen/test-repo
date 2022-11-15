namespace HRMS.Data
{
    using HRMS.Entities;
    using HRMS.Infrastructure.Constants;
    using HRMS.Utilities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public static class MigrationManager
    {
        public static IHost MigrateDatabases(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string ssoConnection = configuration.GetConnectionString("sqlLiteConnection");

                var dbPath = ssoConnection.Split(Delimiters.EqualsTo).Last();

                var dirPath = PathHelper.GetDirectoryName(dbPath);
                if (!string.IsNullOrWhiteSpace(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
                // scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                //using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                //{
                //    try
                //    {
                //        context.Database.Migrate();

                //        if (!context.Clients.Any())
                //        {
                //            foreach (var client in InMemoryConfig.GetClients(configuration))
                //            {
                //                context.Clients.Add(client.ToEntity());
                //            }
                //            context.SaveChanges();
                //        }

                //        if (!context.IdentityResources.Any())
                //        {
                //            foreach (var resource in InMemoryConfig.GetIdentityResources())
                //            {
                //                context.IdentityResources.Add(resource.ToEntity());
                //            }
                //            context.SaveChanges();
                //        }

                //        if (!context.ApiResources.Any())
                //        {
                //            foreach (var resource in InMemoryConfig.GetApiResources())
                //            {
                //                context.ApiResources.Add(resource.ToEntity());
                //            }
                //            context.SaveChanges();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex);
                //        //Log errors or do anything you think it's needed
                //        throw;
                //    }
                //}
            }

            return host;
        }

        public static IHost CreateRolesAndAdminUser(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                foreach (string roleName in Roles.All)
                {
                    CreateRole(scope.ServiceProvider, roleName);
                }

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string adminUserEmail = configuration.GetValue<string>("AppSettings:adminUserEmail");
                string adminPwd = configuration.GetValue<string>("AppSettings:adminPwd");
                if (adminUserEmail == null)
                {
                    throw new ArgumentNullException("AppSettings:adminUserEmail");
                }

                if (adminPwd == null)
                {
                    throw new ArgumentNullException("AppSettings:adminUserEmail");
                }

                createUser(scope.ServiceProvider, "Administrator", adminUserEmail, adminPwd, Roles.Admin);
            }
            return host;
        }

        private static void CreateRole(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                Task<IdentityResult> roleResult = roleManager.CreateAsync(new ApplicationRole() { Name = roleName });
                roleResult.Wait();
            }
        }

        private static void createUser(IServiceProvider serviceProvider, string fullName, string userEmail,
            string userPwd, string roleName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            Task<ApplicationUser> checkAppUser = userManager.FindByEmailAsync(userEmail);
            checkAppUser.Wait();

            if (checkAppUser.Result == null)
            {
                ApplicationUser newAppUser = new ApplicationUser
                {
                    Email = userEmail,
                    UserName = userEmail,
                    FullName = fullName,
                    EmailConfirmed = true,
                    UserStatus = UserStatus.Active,
                    ProfileStatus = ProfileStatus.None

                };

                Task<IdentityResult> taskCreateAppUser = userManager.CreateAsync(newAppUser, userPwd);
                taskCreateAppUser.Wait();

                if (taskCreateAppUser.Result.Succeeded)
                {
                    if (!roleManager.RoleExistsAsync(roleName).Result)
                    {
                        roleManager.CreateAsync(new ApplicationRole { Name = roleName }).Wait();
                    }

                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(newAppUser, roleName);
                    newUserRole.Wait( );
                }
                else
                {
                    throw new Exception(string.Join("\n", taskCreateAppUser.Result.Errors));
                }
            }
        }
    }
}
