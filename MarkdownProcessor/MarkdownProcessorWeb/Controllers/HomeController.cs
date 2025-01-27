using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MarkdownProcessorWeb.Models;

namespace MarkdownProcessorWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}