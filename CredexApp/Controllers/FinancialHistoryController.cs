/* Author: Honiya Maqsood | Description: Functionality for the Financial History view and Details view */

using CredexApp.BusinessLogic;
using CredexApp.Models;
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

        // Add new AnnualReport entry
        [HttpGet]
        public IActionResult AddReport() { 
            return View();
        }
        [HttpPost]
        public IActionResult AddReport(AnnualReport report) { 
            bool yearExists = _financialService.CheckYearExists(report.Year); // Check if year already exists

            // Show error if year already exists
            if (yearExists) {
                ModelState.AddModelError("Year", $"An annual report for {report.Year} already exists.");
            }

            if (ModelState.IsValid) { 
                _financialService.AddAnnualReport(report);
                return RedirectToAction("Index");
            }

            return View(report); // Return to view with validation errors
        }
    }
}
