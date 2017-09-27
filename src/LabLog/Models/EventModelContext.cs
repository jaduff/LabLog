using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LabLog.Domain.Events;

namespace LabLog
{
    public class EventModelContext : DbContext
    {
        //public DbSet<RoomModel> Rooms { get; set; }
        //public DbSet<ComputerModel> Computers { get; set; }
        public DbSet<RoomCreatedEvent> RoomCreatedEvent {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./LabLog.db");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Computer>().HasKey(c => c.ComputerId);
            //modelBuilder.Entity<RoomModel>().HasKey(c => c.Name);
            modelBuilder.Entity<ILabEvent>().HasKey(c => c.RoomId);
            modelBuilder.Entity<ILabEvent>().HasKey(c => c.Version);
        
        }
    }
}