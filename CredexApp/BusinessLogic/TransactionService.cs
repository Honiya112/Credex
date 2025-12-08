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

    // Create a transaction
    public void CreateTransaction(Transactions transaction)
    {
        _db.Transactions.Add(transaction);
        _db.SaveChanges();
    }

    // READ
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