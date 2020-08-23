using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

namespace CheckoutProj.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if(modelBuilder == null)
            {
                return;
            }
            modelBuilder.Entity<User>()
                .HasMany(u => u.WorkingIn)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserID);

            modelBuilder.Entity<Work>()
                .HasOne(p => p.User)
                .WithMany(b => b.WorkingIn)
                .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<Work>()
                .HasOne(p => p.Facility)
                .WithMany(b => b.Workers)
                .HasForeignKey(p => p.FacilityID);

            modelBuilder.Entity<Work>()
                .HasOne(p => p.CurrentPlace)
                .WithMany(b => b.WorkersIn)
                .HasForeignKey(p => p.CurrentPlaceID);

            modelBuilder.Entity<Place>()
                .HasOne(p => p.Facility)
                .WithMany(b => b.Places)
                .HasForeignKey(p => p.FacilityID);

            modelBuilder.Entity<Schedule>()
                .HasOne(p => p.Place)
                .WithMany(b => b.Schedules)
                .HasForeignKey(p => p.PlaceID);

            modelBuilder.Entity<Schedule>()
                .HasOne(p => p.Disease)
                .WithMany(b => b.Schedules)
                .HasForeignKey(p => p.DiseaseID);

            modelBuilder.Entity<Checkout>()
                .HasOne(p => p.User)
                .WithMany(b => b.Checkouts)
                .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<Checkout>()
                .HasOne(p => p.Place)
                .WithMany(b => b.Checkouts)
                .HasForeignKey(p => p.PlaceID);

            modelBuilder.Entity<Checkout>()
                .HasOne(p => p.Disease)
                .WithMany(b => b.Checkouts)
                .HasForeignKey(p => p.DiseaseID);
        }
    }
}
