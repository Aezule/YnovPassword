using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace YnovPassword.Modele
{
    public class PasswordContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=DBYnovPassword.db");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Account>().Property(a => a.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Account>().Property(a => a.URL).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Account>().Property(a => a.Login).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Account>().Property(a => a.TypeProfile).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Account>().Property(a => a.Password).HasMaxLength(200);
        }


    }
}
