/* Author: Honiya Maqsood | Description: Functionality for the Financial History view and Details view */

using CredexApp.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers {
    public class FinancialHistoryController : Controller {
        private FinancialService _financialService;

        public FinancialHistoryController(FinancialService financialService) {
            _financialService = financialService;
        }

        // Display the Financial History main view
        public IActionResult Index(SortOptions sortOption = SortOptions.YearDescending) {
            var annualReports = _financialService.GetAllReports(sortOption);
            return View(annualReports);
        }

        // Display the Details view for a specific year
        public IActionResult Details(int id) { 
            var annualReport = _financialService.GetAnnualReportByYear(id);
            return View(annualReport);
        }
    }
}
