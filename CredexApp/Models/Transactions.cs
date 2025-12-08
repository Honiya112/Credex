using System.ComponentModel.DataAnnotations;

namespace CredexApp.Models;

public class Transactions
{

    [Key]
    public int TransactionId { get; set; }
    public double Spending { get; set; } // y axis
    public double Earnings { get; set; } // y axis
    public DateTime TransactionDate { get; set; } // used for chart data (x axis)
}