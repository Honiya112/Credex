/* Author: Honiya Maqsood | Description: Class object representation of a row in the CSV file. */

namespace CredexApp.Models {
    public class AnnualReport {
        public int Id { get; set; }
        public int Year { get; set; }
        public decimal Revenue { get; set; } // Total income w/out expenses subtracted | in Billions
        public decimal Earnings { get; set; } // Income before taxes | in Billions
        public decimal TotalAssets { get; set; } // Everything owned by the company | in Billions
        public decimal TotalDebt { get; set; } // All current/non-current debts | in Billions
        public decimal TotalLiabilities { get; set; } // All current/non-current debts and obligations | in Billions
    }
}
