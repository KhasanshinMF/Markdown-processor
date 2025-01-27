using System.Security.Claims;
using MarkdownProcessorWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MarkdownProcessorWeb.Controllers;

public class MainController : Controller
{
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var email = User.FindFirstValue(ClaimTypes.Email);
        
        var model = new MainViewModel
        {
            UserId = userId,
            Username = username,
            Email = email
        };

        return View(model);
    }
}