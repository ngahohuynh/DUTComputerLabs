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
            
            builder.Entity<Booking>()
                .HasOne<Feedback>(b => b.Feedback)
                .WithOne(f => f.Booking)
                .HasForeignKey<Feedback>(f => f.BookingId);
        }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<ComputerLab> ComputerLabs { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}