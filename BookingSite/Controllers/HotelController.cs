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
using BookingSite.ViewModels.Interface;
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
        _client.BaseAddress = new Uri("https://api.content.tripadvisor.com/api/v1");
        _userManager = userManager;
        _bookingService = bookingService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var value = await _bookingService.GetCityLattitudeLongitudeOfLastBookedTicketsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        Console.WriteLine(value);
        List<IRootObject<HotelViewModel>> data = new List<IRootObject<HotelViewModel>>();
        foreach (var item in value)
        {
          data.Add(await MakeApiRequest<HotelViewModel>($"location/search?key=34B9C74963ED491BBC3D7A5817EF2814&searchQuery=hotel&latLong={item}&language=en"));
        }
        return View(data);
    }
    

    public async Task<IRootObject<T>> MakeApiRequest<T>(string endpoint)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_client.BaseAddress}/{endpoint}"),
            Headers = { { "accept", "application/json" } }
        };

        using (var response = await _client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            
            var settings = new JsonSerializerSettings{ TypeNameHandling = TypeNameHandling.Auto };
            var result = JsonConvert.DeserializeObject<RootObject<T>>(body, settings);
            return result;
        }
    }

}