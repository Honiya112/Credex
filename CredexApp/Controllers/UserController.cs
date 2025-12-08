using CredexApp.BusinessLogic;
using CredexApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers;


public class UserController : Controller
{
    // this controller will allow the Admin to 
    private readonly TransactionService _transactionService;
  
    public UserController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int? page = 1)
    {
        var transactions = await _transactionService.GetTransactionsAsync(page, 50);
        var chartData = await _transactionService.GetChartDataAsync();

        ViewBag.ChartData = chartData;
        
        return View(transactions);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View(new Transactions());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Transactions transaction)
    {
        if (ModelState.IsValid)
        {
            await _transactionService.CreateTransactionsAsync(transaction);
            return RedirectToAction("Index");
        }
        return View(transaction);
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTransaction(int transactionId)
    {
        var success = await _transactionService.DeleteTransactionAsync(transactionId);
        if (!success)
        {
            TempData["Error"] = "Transaction not found";
        }
        return RedirectToAction("Index");
    }
}