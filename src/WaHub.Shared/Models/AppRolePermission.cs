using System;

namespace WaHub.Shared.Models
{
    public class AppRolePermission
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public AppRole Role { get; set; }
        public string Permission { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public static class AppPermissions
    {
        public const string ManageUsers = "manage_users";
        public const string ManageRoles = "manage_roles";
        public const string ViewDashboard = "view_dashboard";
        public const string ManageCampaigns = "manage_campaigns";
        public const string ManageInstances = "manage_instances";
        public const string ViewReports = "view_reports";
        public const string ManageSettings = "manage_settings";
    }
}
