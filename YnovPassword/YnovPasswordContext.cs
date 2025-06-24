using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace YnovPassword
{
    public class PasswordContext : DbContext
    {
        public DbSet<Models.Account> Accounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=YnovPassword.db");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Account>().ToTable("Accounts");
            modelBuilder.Entity<Models.Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Models.Account>().Property(a => a.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Models.Account>().Property(a => a.URL).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Models.Account>().Property(a => a.Login).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Models.Account>().Property(a => a.TypeProfile).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Models.Account>().Property(a => a.Password).HasMaxLength(200);
        }

 
    }
}