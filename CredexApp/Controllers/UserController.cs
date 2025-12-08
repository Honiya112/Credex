using CredexApp.BusinessLogic;
using CredexApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CredexApp.Controllers;

public class UserController : Controller
{
    
    private UserService _service;
  
    public UserController(UserService service)
    {
        _service = service;
    }

}