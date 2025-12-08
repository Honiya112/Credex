using CredexApp.BusinessLogic;
using CredexApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers;

public class UserController : Controller
{
    // this controller will allow the Admin to 
    private TransactionService _transactionService;
  
    public UserController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(Transactions transaction)
    {
        if (!ModelState.IsValid)
        {
            return NoContent();
        }

        await _transactionService.GetChartDataAsync();
        return View(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction(Transactions transaction)
    {
       await _transactionService.CreateTransactionsAsync(transaction);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditTransaction(int transactionId)
    {
        var transaction = await _transactionService.FindTransactionAsync(transactionId);

        if (transaction == null)
        {
            return NotFound();
        }
        
        return View(transaction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTransaction(int transactionId, Transactions transaction)
    {
        if (transactionId != transaction.TransactionId)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var success = await _transactionService.UpdateTransactionAsync(transactionId, transaction);
            if (success)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "An error occured");
        }
        return View(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTransaction(int transactionId)
    {
        var transaction = await _transactionService.DeleteTransactionAsync(transactionId);
        return RedirectToAction("Index");
    }
}