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
        return View(await GetHotels("40.71427000,-74.00597000"));
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

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RootObject>(body);
            return result;
        }
    }
}