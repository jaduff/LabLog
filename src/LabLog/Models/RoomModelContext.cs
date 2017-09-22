using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LabLog.Models;

namespace LabLog
{
    public class RoomModelContext : DbContext
    {
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<ComputerModel> Computers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=./sqlite/LabLog.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComputerModel>().HasKey(c => c.SerialNumber);
            modelBuilder.Entity<RoomModel>().HasKey(c => c.Name);
        }
    }
}