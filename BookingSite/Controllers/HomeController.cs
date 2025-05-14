using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using BookingSite.ViewModels;

namespace BookingSite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<HomeController> _localizer;

    public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = _localizer["dailyFlights"];
        ViewData["Message"] = _localizer["Message"];
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpPost]
    public IActionResult SetAppLanguage(string lang, string returnUrl)
    {
        Console.WriteLine("mip");
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(lang)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            }
        );
        return LocalRedirect(returnUrl);
    }

    // Add this to your HomeController
    public IActionResult TestNotFound()
    {
        return NotFound();
    }

    public IActionResult TestForbidden()
    {
        return Forbid();
    }

    public IActionResult TestError()
    {
        throw new Exception("This is a test exception");
    }
}

