namespace CredexApp.Models;

public class Transactions
{
    public int TransactionId { get; set; }
    public int Spending { get; set; } // y axis
    public int Earnings { get; set; } // y axis
    public DateTime TransactionDate { get; set; } // used for chart data (x axis)
}