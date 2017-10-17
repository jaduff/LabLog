using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LabLog.Domain.Events;
using LabLog.Models;

namespace LabLog
{
    public class EventModelContext : DbContext
    {
        public DbSet<LabEvent> LabEvents { get; set; }
        public DbSet<SchoolModel> Schools { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./LabLog.db");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabEvent>().HasKey(c => new { c.SchoolId, c.Version });
            modelBuilder.Entity<LabEvent>().Property(c => c.EventType).IsRequired();
            modelBuilder.Entity<SchoolModel>().HasKey(c => new { c.Id });
            modelBuilder.Entity<ComputerModel>().HasKey(c => new { c.SerialNumber });
        }

    }
}