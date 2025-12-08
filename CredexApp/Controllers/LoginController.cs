using CredexApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers;

public class LoginController : Controller
{
    
    private SignInManager<User> _signInManager;

    public LoginController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }
    
    // GET
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "User");
        }
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(login);
    }
}