/* Author: Honiya Maqsood | Description: methods used in FinancialHistoryController to display data from dba */

using CredexApp.DataLayer;
using CredexApp.Models;

namespace CredexApp.BusinessLogic {
    public class FinancialService {
        private readonly AppDbContext _context;

        // Get dba connection by dependency injection
        public FinancialService(AppDbContext context) {
            _context = context;
        }

        // Get all AnnualReport from dba based on sort option
        public List<AnnualReport> GetAllReports(SortOptions sortOption) {
            var reports = _context.AnnualReports.ToList();

            switch (sortOption) { 
                case SortOptions.YearAscending:
                    return reports.OrderBy(r => r.Year).ToList();

                case SortOptions.RevenueDescending:
                    return reports.OrderByDescending(r => r.Revenue).ToList();

                case SortOptions.EarningsDescending:
                    return reports.OrderByDescending(r => r.Earnings).ToList();

                case SortOptions.YearDescending:
                default:
                    return reports.OrderByDescending(r => r.Year).ToList();
            }
        }

        // Get AnnualReport by year
        public AnnualReport GetAnnualReportByYear(int id) {
            return _context.AnnualReports.FirstOrDefault(r => r.Id == id);
        }

        // Add AnnualReport to dba
        public void AddAnnualReport(AnnualReport report) {
            _context.AnnualReports.Add(report);
            _context.SaveChanges();
        }

        // Check if AnnualReport for a year already exists
        public bool CheckYearExists(int year) {
            return _context.AnnualReports.Any(r => r.Year == year);
        }
    }
}
