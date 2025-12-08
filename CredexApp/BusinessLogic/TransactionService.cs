using CredexApp.DataLayer;
using CredexApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CredexApp.BusinessLogic;

public class TransactionService
{
    private readonly AppDbContext _db;
    
    public TransactionService(AppDbContext db)
    {
        _db = db;
    }

    // SEARCH

    public async Task<Transactions?> FindTransactionAsync(int transactionId)
    {
       return await  _db.Transactions.FindAsync(transactionId);
    }
    
    // READ with pagination (incase of large data)
    public async Task<List<Transactions>> GetTransactionsAsync(int? page = 1, int pageSize = 50)
    {
        try
        {
            return await _db.Transactions
                .OrderByDescending(t => t.TransactionDate) //recent first
                .Skip((page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception e)
        {
            var errorMessage = $"Error: {e.Message}";
            
            return new List<Transactions>();
        }
    }
    
    //READ FOR CHART DATA
    public async Task<object> GetChartDataAsync(DateTime? fromDate = null)
    {
        var query = _db.Transactions.AsQueryable();
        if (fromDate.HasValue)
            query = query.Where(t => t.TransactionDate.Date >= fromDate.Value.Date);
        
        var data = await query
            .GroupBy(t => t.TransactionDate.Date)
            .Select(g => new
            {
                date = g.Key, // cannot read format yyyy-MM-dd, changed to g.Key
                spending = g.Sum(t => t.Spending),
                earnings = g.Sum(t => t.Earnings),
                
            })
            .OrderBy(x => x.date)
            .ToListAsync();

        return new
        {
            labels = data.Select(x => x.date),
            datasets = new[]
            {
                new
                {
                    label = "Spending", data = data.Select(x => (double)x.spending),
                    borderColor = "rgba(255, 99, 123, 1)", backgroundColor = "rgba(255, 99, 132, 0.2)"
                },
                new
                {
                    label = "Earnings", data = data.Select(x => (double)x.earnings),
                    borderColor = "rgba(75, 192, 192, 1)", backgroundColor = "rgba(75, 192, 192, 0.2)"
                }
            }
        };
    }
    
    // CREATE
    public async Task<int> CreateTransactionsAsync(Transactions transaction)
    {
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();
        return transaction.TransactionId;
    }
    
    // UPDATE
    public async Task<bool> UpdateTransactionAsync(int transactionId, Transactions updatedTransaction)
    {
        var transaction = await _db.Transactions.FindAsync(transactionId);
        if (transaction == null)
        {
            return false;
        }
        
        _db.Entry(transaction).CurrentValues.SetValues(updatedTransaction);
        await _db.SaveChangesAsync();
        return true;
    }
    
    // DELETE
    public async Task<bool> DeleteTransactionAsync(int transactionId)
    {
        var transaction = await _db.Transactions.FindAsync(transactionId);
        if (transaction == null)
        {
            return false;
        }
        
        _db.Transactions.Remove(transaction);
        await _db.SaveChangesAsync();
        return true;
    }
}