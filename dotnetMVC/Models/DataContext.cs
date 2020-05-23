using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnetMVC.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Class>().HasData(new Class { ClassId = 1, ClassName = "18T1" });
            builder.Entity<Class>().HasData(new Class { ClassId = 2, ClassName = "18T2" });

            builder.Entity<Student>().HasData(new Student { StudentId = 1, ClassId = 1, StudentName = "Peter", BirthDate = new DateTime(1999, 1, 1) });
            builder.Entity<Student>().HasData(new Student { StudentId = 2, ClassId = 2, StudentName = "David", BirthDate = new DateTime(1999, 1, 1) });
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
    }
}