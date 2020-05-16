using System.ComponentModel.DataAnnotations.Schema;

namespace DUTComputerLabs.API.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}