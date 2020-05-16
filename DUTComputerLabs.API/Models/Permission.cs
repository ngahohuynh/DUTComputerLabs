using System.Collections.Generic;

namespace DUTComputerLabs.API.Models
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}