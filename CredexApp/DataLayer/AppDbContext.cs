/* Author: Honiya Maqsood | Description: Connection to the database which will store annual reports and transactions */

using CredexApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CredexApp.DataLayer {
    public class AppDbContext : IdentityDbContext<User> {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

        public DbSet<AnnualReport> AnnualReports { get; set; } // List of annual reports

        public DbSet<Transactions> Transactions { get; set; } // List of transactions
        
        // this will fix the error: Unhandled exception. System.InvalidOperationException: Cannot create a DbSet for 'User' because this type is not included in the model for the context.
        // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0#customize-the-model

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // this automatically registers Users, Roles, etc
        }
    }
}
