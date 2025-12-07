/* Author: Honiya Maqsood | Description: Reads the CSV file, maps columns to fields in AnnualReport, saves to dba */

using CredexApp.DataLayer;
using CredexApp.Models;

namespace CredexApp.BusinessLogic {
    public class CsvImportService {
        private readonly AppDbContext _context;

        // Get dba connection by dependency injection
        public CsvImportService(AppDbContext context) {
            _context = context;
        }

        // Import data from CSV file and into dba as AnnualReport objects
        public void ImportData(string filePath) {
            // Check if loaded already, if so, skip import
            if (_context.AnnualReports.Any()) {
                return;
            }

            // Read file
            var lines = System.IO.File.ReadAllLines(filePath);

            foreach (var line in lines.Skip(1)) { // Skip header
                var columns = line.Split(','); // Split entries by comma

                // Map needed columns to AnnualReport fields
                var report = new AnnualReport {
                    Year = int.Parse(columns[0]),
                    Revenue = decimal.Parse(columns[2]),
                    Earnings = decimal.Parse(columns[3]),
                    TotalAssets = decimal.Parse(columns[14]),
                    TotalDebt = decimal.Parse(columns[15]),
                    TotalLiabilities = decimal.Parse(columns[16])
                };
                _context.AnnualReports.Add(report); // Add to dba context
            }
            _context.SaveChanges(); // Save dba changes
        }

        // Get all AnnualReport from dba
        public List<AnnualReport> GetAllReports() {
            return _context.AnnualReports.ToList();
        }

        // Get AnnualReport by year
        public AnnualReport GetAnnualReportByYear(int id) { 
            return _context.AnnualReports.FirstOrDefault(r => r.Id == id);
        }
    }
}
