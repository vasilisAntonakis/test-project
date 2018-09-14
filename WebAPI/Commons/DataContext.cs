using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAPI.Model;

namespace WebAPI.Data
{
    /// <summary>
    /// This is the actual concrete DbContext of the app.
    /// It registers the name of the initial catalog (DB) = "test_project"
    /// and the tables this DB will have (DbSets).
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext() : base("test_project") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().Property(p => p._LoginRecEnded);
            modelBuilder.Entity<Title>().Property(p => p._LoginRecStarted);
            modelBuilder.Entity<Title>().Property(p => p._LoginRecEnded);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Lawyer> Lawyers { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Title> Titles { get; set; }
    }
}