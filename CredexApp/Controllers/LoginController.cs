using CredexApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers;

[AllowAnonymous]
[Route("Login")]
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Login login, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        var result = await _signInManager.PasswordSignInAsync(
            login.Username, 
            login.Password, 
            login.RememberMe, 
            false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "User");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(login);
    }
}