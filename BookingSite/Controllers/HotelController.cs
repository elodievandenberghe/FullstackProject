using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using BookingSite.Data;
using BookingSite.Domains.Models;
using BookingSite.Repositories.Interfaces;
using BookingSite.Services.Interfaces;
using BookingSite.ViewModels;
using BookingSite.ViewModels.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingSite.Controllers;

public class HotelController : Controller
{
    private static HttpClient _client = new HttpClient();
    private IBookingService _bookingService;
    private readonly TripAdvisorApiKey _tripAdvisorApiKey;
    private readonly IService<City, int> _cityService;
    
    public HotelController(IBookingService bookingService, IOptions<TripAdvisorApiKey> tripAdvisorApiKey, IService<City, int> cityService)
    {
        if (_client.BaseAddress == null)
        {
            _client.BaseAddress = new Uri("https://api.content.tripadvisor.com/api/v1");
        }
        _bookingService = bookingService;
        _tripAdvisorApiKey = tripAdvisorApiKey.Value;
        _cityService = cityService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        try
        {
            var value = await _bookingService.GetCityLattitudeLongitudeOfLastBookedTicketsAsync(
                User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            if (value.IsNullOrEmpty())
            {
                return View("NoBookings");
            }
            
            List<HotelViewModel> data = new List<HotelViewModel>();

            foreach (var item in value)
            {
                var response = await MakeApiRequest<HotelViewModel>(
                    $"location/search?key={_tripAdvisorApiKey.ApiKey}&searchQuery=hotel&latLong={item}&language=en");
                data.AddRange(response.Data.Select(d => d).ToList());
            }

            foreach (var item in data)
            {
                var imageurl = MakeApiRequest<HotelImageViewModel>(
                    $"location/{item.LocationId}/photos?language=en&key={_tripAdvisorApiKey.ApiKey}");


                var weburl = ViewInfo(
                    $"location/{item.LocationId}/details?&key={_tripAdvisorApiKey.ApiKey}");

                await Task.WhenAll(imageurl, weburl);
                item.ImageUrl = imageurl?.Result.Data?[0]?.Images?.Original?.Url ?? null;
                item.WebUrl = weburl.Result.WebUrl ?? null;
            }

            return View(data);
        }
        catch (Exception ex)
        {
            return View("NoBookings");
        }
    }
    
    [Authorize]
    [Route("Hotel/{cityId:int}")]
    public async Task<IActionResult> Index(int? cityId)
    {
        try
        {
            IEnumerable<string> value;

            if (cityId == null)
            {
                return View("NoBookings");
            }
            var city = await _cityService.FindByIdAsync(cityId.Value);
            value = new List<string> { city?.LatLong ?? "" };

            var data = new List<HotelViewModel>();
            foreach (var item in value)
            {
                var response = await MakeApiRequest<HotelViewModel>(
                    $"location/search?key={_tripAdvisorApiKey.ApiKey}&searchQuery=hotel&latLong={item}&language=en");
                data.AddRange(response.Data.Select(d => d).ToList());
            }

            foreach (var item in data)
            {
                var imageurl = MakeApiRequest<HotelImageViewModel>(
                    $"location/{item.LocationId}/photos?language=en&key={_tripAdvisorApiKey.ApiKey}");


                var weburl = ViewInfo(
                    $"location/{item.LocationId}/details?&key={_tripAdvisorApiKey.ApiKey}");

                await Task.WhenAll(imageurl, weburl);
                item.ImageUrl = imageurl?.Result.Data?[0]?.Images?.Original?.Url ?? null;
                item.WebUrl = weburl.Result.WebUrl ?? null;
            }

            return View(data);
        }
        catch (Exception ex)
        {
            return View("NoBookings");
        }
    }
    
    
    public async Task<RootObject<T>> MakeApiRequest<T>(string endpoint)
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

    public async Task<HotelInfoViewModel> ViewInfo(string endpoint)
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
            var result = JsonConvert.DeserializeObject<HotelInfoViewModel>(body, settings);
            return result;
        }
    }

}