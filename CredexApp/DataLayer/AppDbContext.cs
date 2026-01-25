/* Author: Honiya Maqsood | Description: Connection to the database which will store annual reports and transactions */

using CredexApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CredexApp.DataLayer {
    public class AppDbContext : IdentityDbContext<User> {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AnnualReport> AnnualReports { get; set; } // List of annual reports

        public DbSet<Transactions> Transactions { get; set; } // List of transactions

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder); // this automatically registers Users, Roles, etc

            // Indexing --> sort lists by Date in SQLite
            modelBuilder.Entity<Transactions>().HasIndex(t => t.TransactionDate);

            // Search by Category
            modelBuilder.Entity<Transactions>().HasIndex(t => t.Category);

            // Precise Config. 
            // TODO: HOW IS THIS DIFFERENT FROM AMOUNT??
            modelBuilder.Entity<Transactions>()
                .Property(t => t.Spending)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AnnualReport>()
                .Property(a => a.Revenue)
                .HasPrecision(18, 2);
        }
    }
}
