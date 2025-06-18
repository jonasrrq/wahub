using System;
using System.Collections.Generic;

namespace WaHub.Shared.Models
{
    public class AppRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AppRolePermission> Permissions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
