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
        List<HotelViewModel> hotels = new List<HotelViewModel>();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={destId}&search_type=CITY&arrival_date={DateTime.Today}&departure_date={DateTime.Today.AddDays(1)}&adults=1&room_qty=1&page_number=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=AED&location=US"),
            Headers =
            {
                { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
                { "x-rapidapi-key", "95516cd78cmshacb5723ea0358dcp1421cbjsn570caeee9830" }
            }
        };

        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<HotelViewModel>>(body);
            Console.WriteLine(body);
        }

        return View(hotels);
    }
}