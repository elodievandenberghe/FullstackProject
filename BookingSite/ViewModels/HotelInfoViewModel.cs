using BookingSite.ViewModels.Interface;
using Newtonsoft.Json;

namespace BookingSite.ViewModels;

public class RootHotelnfoObject : IRootObject<HotelInfoViewModel>
{
    [JsonProperty("data")]
    public List<HotelInfoViewModel> Data { get; set; }
}

public class HotelInfoViewModel
{
    [JsonProperty("web_url")]
    public string? WebUrl { get; set; }
}