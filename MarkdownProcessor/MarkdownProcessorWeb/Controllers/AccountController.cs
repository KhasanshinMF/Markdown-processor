using System.Security.Claims;
using MarkdownProcessorWeb.Models;
using MarkdownProcessorWeb.Services;
using MarkdownProcessorWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace MarkdownProcessorWeb.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _authService.RegisterAsync(model.Email, model.Username, model.Password);
                await SignInUserAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Greeting");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _authService.LoginAsync(model.Email, model.Password);
                await SignInUserAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Document");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
        }

        return View(model);
    }
    
    [HttpGet]
    public IActionResult LoginWithGoogle()
    {
        var redirectUrl = Url.Action("GoogleLoginCallback", "Account");
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl,
            Parameters =
            {
                { "prompt", "select_account" }
            }
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet]
    public async Task<IActionResult> GoogleLoginCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return RedirectToAction("Login");

        var email = result.Principal.FindFirstValue(ClaimTypes.Email);
        var name = result.Principal.FindFirstValue(ClaimTypes.Name);
        
        var user = await _authService.GetUserByEmailAsync(email);

        if (user == null)
        {
            user = await _authService.RegisterAsync(email, name, "google_password");
        }

        await SignInUserAsync(user, false);
        return RedirectToAction("Index", "Document");
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Greeting");
    }

    private async Task SignInUserAsync(User user, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
    }
}