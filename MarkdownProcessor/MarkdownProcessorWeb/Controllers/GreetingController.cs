using Microsoft.AspNetCore.Mvc;

namespace MarkdownProcessorWeb.Controllers;

public class GreetingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}