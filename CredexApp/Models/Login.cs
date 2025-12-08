using System.ComponentModel.DataAnnotations;

namespace CredexApp.Models;

public class Login
{
    [Required(ErrorMessage = "Please enter your username")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
    
}