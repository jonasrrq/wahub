using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WaHub.Data;
using WaHub.Shared.Models;

namespace WaHub.Services
{
    public interface IRoleService
    {
        Task<List<AppRole>> GetAllRolesAsync();
        Task<AppRole> GetRoleByIdAsync(Guid id);
        Task<AppRole> CreateRoleAsync(AppRole role);
        Task<AppRole> UpdateRoleAsync(AppRole role);
        Task DeleteRoleAsync(Guid id);
        Task<List<string>> GetUserPermissionsAsync(string userId);
    }

    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await _context.AppRoles
                .Include(r => r.Permissions)
                .ToListAsync();
        }

        public async Task<AppRole> GetRoleByIdAsync(Guid id)
        {
            return await _context.AppRoles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<AppRole> CreateRoleAsync(AppRole role)
        {
            role.Id = Guid.NewGuid();
            role.CreatedAt = DateTime.UtcNow;
            
            _context.AppRoles.Add(role);
            await _context.SaveChangesAsync();
            
            return role;
        }

        public async Task<AppRole> UpdateRoleAsync(AppRole role)
        {
            var existingRole = await GetRoleByIdAsync(role.Id);
            if (existingRole == null)
                throw new KeyNotFoundException($"Role with ID {role.Id} not found");

            existingRole.Name = role.Name;
            existingRole.Description = role.Description;
            existingRole.UpdatedAt = DateTime.UtcNow;

            // Update permissions
            _context.AppRolePermissions.RemoveRange(existingRole.Permissions);
            foreach (var permission in role.Permissions)
            {
                permission.Id = Guid.NewGuid();
                permission.RoleId = existingRole.Id;
                permission.CreatedAt = DateTime.UtcNow;
            }
            existingRole.Permissions = role.Permissions;

            await _context.SaveChangesAsync();
            return existingRole;
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {id} not found");

            _context.AppRoles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new List<string>();

            return user.Roles
                .SelectMany(r => r.Permissions)
                .Select(p => p.Permission)
                .Distinct()
                .ToList();
        }
    }
}
