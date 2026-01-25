using CredexApp.BusinessLogic;
using CredexApp.DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers {
    public class StatsController : Controller {
        private readonly AppDbContext _context;
        private readonly StatisticsService _statsService;

        public StatsController(AppDbContext context, StatisticsService statsService) {
            _context = context;
            _statsService = statsService;
        }

        public IActionResult Index() { 
            return View();    
        }

        // Endpoint to get monthly spending data for Chart.js
        [HttpGet]
        public IActionResult GetFinancialData() { 
            // Get raw data
            var reports = _context.AnnualReports.OrderBy(r => r.Year).ToList();

            var predictedEarnings = _statsService.PredictNextYearEarnings(reports);

            var chartData = new { 
                years = reports.Select(r => r.Year).ToList(),
                revenues = reports.Select(r => r.Revenue).ToList(),
                earnings = reports.Select(r => r.Earnings).ToList(),

                prediction = new { 
                    year = reports.Last().Year + 1,
                    value = predictedEarnings
                }
            };

            // Return JSON that Chart.js can use
            return Json(chartData);
        }
    }
}
