/* Author: Honiya Maqsood | Description: Functionality for the Financial History view and Details view */

using CredexApp.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers {
    public class FinancialHistoryController : Controller {
        private CsvImportService _importService;

        FinancialHistoryController(CsvImportService importService) {
            _importService = importService;
        }

        // Display the Financial History main view
        public IActionResult Index() {
            var annualReports = _importService.GetAllReports();
            return View(annualReports);
        }

        // Display the Details view for a specific year
        public IActionResult Details(int year) { 
            var annualReport = _importService.GetAnnualReportByYear(year);
            return View(annualReport);
        }
    }
}
