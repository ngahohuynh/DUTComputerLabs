using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DUTComputerLabs.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            builder.Entity<RolePermission>()
                    .HasOne<Role>(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId);
            builder.Entity<RolePermission>()
                    .HasOne<Permission>(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<Booking>()
                .HasOne<Feedback>(b => b.Feedback)
                .WithOne(f => f.Booking)
                .HasForeignKey<Feedback>(f => f.BookingId);
        }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<ComputerLab> ComputerLabs { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}