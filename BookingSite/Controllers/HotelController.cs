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

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetHotels(int destId)
    {
        List<HotelViewModel> hotels = new List<HotelViewModel>();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={destId}&search_type=CITY&arrival_date={DateTime.Today}&departure_date={DateTime.Today.AddDays(1)}&adults=1&room_qty=1&page_number=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=AED&location=US"),
            Headers =
            {
                { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
                { "x-rapidapi-key", "random_api_key" }
            }
        };

        using (var response = await client.SendAsync(request))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON into your model
                // hotels = JsonConvert.DeserializeObject<List<HotelViewModel>>(json);
                // Assuming the API returns a list you can map
            }
        }

        return View(hotels);
    }
}