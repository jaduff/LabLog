using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LabLog.Domain.Events;

namespace LabLog
{
    public class EventModelContext : DbContext
    {
        public DbSet<LabEvent> LabEvents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./LabLog.db");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LabEvent>().HasKey(c => c.RoomId);
            modelBuilder.Entity<LabEvent>().HasKey(c => c.Version);
        
        }
    }
}