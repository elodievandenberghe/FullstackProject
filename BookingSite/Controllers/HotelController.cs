using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using BookingSite.ViewModels;

namespace BookingSite.Controllers;

public class HotelController : Controller
{
    private static HttpClient client = new HttpClient();

    public async  Task<IActionResult> Index()
    {
        await GetHotels(20088325);
        return View();
    }

    public async Task<IActionResult> GetHotels(int destId)
    {
        return View();
    }
}