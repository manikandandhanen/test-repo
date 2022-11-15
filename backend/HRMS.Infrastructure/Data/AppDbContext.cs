using HRMS.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Data
{
    // IdentityDbContext contains all the user tables
    public class AppDbContext
    //: IdentityDbContext<ApplicationUser>
        : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserBasicInfo> UserBasicInfos { get; set; }
        public DbSet<UserContactInfo> UserContactInfos { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                       .HasOne<UserProfile>(p => p.UserProfile)
                       .WithOne(s => s.User);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });


            builder.Entity<UserProfile>(userProfile =>
            {
                userProfile.HasKey(s => s.UserId);

                userProfile.HasOne<UserBasicInfo>(p => p.UserBasicInfo)
                            .WithOne(s => s.UserProfile)
                            .HasForeignKey<UserBasicInfo>(s => s.UserId)
                            .OnDelete(DeleteBehavior.ClientCascade);

                userProfile.HasOne<UserContactInfo>(p => p.UserContactInfo)
                            .WithOne(s => s.UserProfile)
                            .HasForeignKey<UserContactInfo>(s => s.UserId)
                            .OnDelete(DeleteBehavior.ClientCascade);

            });


            //builder.Entity<UserProfile>()
            //            .HasKey(s => s.UserId);
           

            builder.Entity<UserBasicInfo>()
                         .HasKey(s => s.UserId);

            //builder.Entity<UserProfile>()
            //            .HasOne<UserBasicInfo>(p => p.UserBasicInfo)
            //            .WithOne(s => s.UserProfile)
            //            .HasForeignKey<UserBasicInfo>(s => s.UserId)
            //            .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<UserContactInfo>()
                        .HasKey(s => s.UserId);

            //builder.Entity<UserProfile>()
            //            .HasOne<UserContactInfo>(p => p.UserContactInfo)
            //            .WithOne(s => s.UserProfile)
            //            .HasForeignKey<UserContactInfo>(s => s.UserId)
            //            .OnDelete(DeleteBehavior.ClientCascade);


            builder.Entity<UserAddress>()
                        .HasKey(s => s.UserAddressId);

            builder.Entity<UserContactInfo>()
            .HasOne(ars => ars.CorrespondenceAddress)
            .WithOne()
            .HasForeignKey<UserContactInfo>(ars => ars.CorrespondenceAddressId)
            .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<UserContactInfo>()
                .HasOne(ars => ars.PermanentAddress)
                .WithOne()
                .HasForeignKey<UserContactInfo>(ars => ars.PermanentAddressId)
                .OnDelete(DeleteBehavior.ClientCascade);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = this.ChangeTracker.Entries()
                                   .Where(x => x.State == EntityState.Added)
                                   .Select(x => x.Entity);

            var currentUsername = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";
            // var currentUsername =  _user ?? "Anonymous";

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as IAuditable;
                //If the inserted object is an Auditable. 
                if (auditableEntity != null)
                {
                    auditableEntity.DateCreated = DateTimeOffset.UtcNow;
                    auditableEntity.UserCreated = currentUsername;
                }

               // var audit = insertedEntry as IAuditable;

            }

            var modifiedEntries = this.ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                //If the inserted object is an Auditable. 
                var auditableEntity = modifiedEntry as Auditable;
                if (auditableEntity != null)
                {
                    auditableEntity.DateModified = DateTimeOffset.UtcNow;
                    auditableEntity.UserModified = currentUsername;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
