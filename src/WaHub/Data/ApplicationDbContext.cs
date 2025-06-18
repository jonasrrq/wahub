using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaHub.Shared.Models;
using System;
using System.Linq;

namespace WaHub.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppRolePermission> AppRolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });

        builder.Entity<AppRolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(e => e.RoleId);
        });

        // Seed default roles with static IDs
        var adminRoleId = new Guid("44546e06-8719-4ad8-b88a-f271ae9d6eab");
        builder.Entity<AppRole>().HasData(
            new AppRole 
            { 
                Id = adminRoleId,
                Name = "Administrator",
                Description = "Full system access",
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed default permissions for admin role with static IDs
        var permissionEntities = new[]
        {
            new AppRolePermission 
            { 
                Id = new Guid("7d9b7113-a8f8-4035-99a7-a20dd400f6a3"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ManageUsers,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRolePermission 
            { 
                Id = new Guid("bb19f453-177d-4817-a5f4-ec91d97706a9"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ManageRoles,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRolePermission 
            { 
                Id = new Guid("e367f655-856f-4b14-a4b9-0f9c58970635"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ViewDashboard,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRolePermission 
            { 
                Id = new Guid("0f147473-7536-4c8c-86c3-8b5a3e2f6624"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ManageCampaigns,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRolePermission 
            { 
                Id = new Guid("d2606a8b-d928-4837-8b4f-3d5c054898c4"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ManageInstances,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRolePermission 
            { 
                Id = new Guid("4647bf49-1f9d-4c9e-a7c7-472aa987d5c5"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ViewReports,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            },
            new AppRolePermission 
            { 
                Id = new Guid("9e6cf6c8-7d0c-4428-b01a-5d68df3c3947"), 
                RoleId = adminRoleId, 
                Permission = AppPermissions.ManageSettings,
                CreatedAt = new DateTime(2025, 6, 18, 0, 0, 0, DateTimeKind.Utc)
            }
        };

        builder.Entity<AppRolePermission>().HasData(permissionEntities);
    }
}
