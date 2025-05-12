using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using BookingSite.Data;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BookingSite.Controllers;

public class HotelController : Controller
{
    private static HttpClient _client = new HttpClient();
    private IBookingService _bookingService;
    private readonly UserManager<ApplicationUser> _userManager;


    public HotelController(IBookingService bookingService, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _bookingService = bookingService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var value = await _bookingService.GetCityLattitudeLongitudeOfLastBookedTicketsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        Console.WriteLine(value);
        List<RootObject> values = new List<RootObject>();

        foreach (var item in value)
        {
            values.Add(await GetHotels(item));
        }
        
        return View(values);
    }

    public async Task<RootObject> GetHotels(string latlong)
    {
        List<HotelViewModel> hotels = new List<HotelViewModel>();
        latlong = latlong.Replace(",", "%2C%20");
        Console.WriteLine(latlong);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://api.content.tripadvisor.com/api/v1/location/search?key=34B9C74963ED491BBC3D7A5817EF2814&searchQuery=hotel&latLong={latlong}&language=en"),
            Headers =
            {
                { "accept", "application/json" }
            }
        };

        using (var response = await _client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RootObject>(body);
            return result;
        }
    }
}