/* Author: Honiya Maqsood | Description: Connection to the database which will store annual reports and transactions */

using CredexApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CredexApp.DataLayer {
    public class AppDbContext : IdentityDbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        public DbSet<AnnualReport> AnnualReports { get; set; } // List of annual reports

        public DbSet<Transactions> Transactions { get; set; } // List of transactions
    }
}
