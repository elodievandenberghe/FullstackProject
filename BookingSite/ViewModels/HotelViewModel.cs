using System.Text.Json.Serialization;
using BookingSite.Domains.Models;
using Newtonsoft.Json;

namespace BookingSite.ViewModels;


public class RootObject
{
    [JsonProperty("data")]
    public List<HotelViewModel> Data { get; set; }
}

public class HotelViewModel
{
    [JsonProperty("location_id")]
    public string? LocationId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("distance")]
    public string? Distance { get; set; }

    [JsonProperty("bearing")]
    public string? Bearing { get; set; }

    [JsonProperty("address_obj")]
    public AddressObject Address { get; set; }  // FIXED: should be a single object, not a list
}


public class AddressObject
{
    [JsonProperty("street1")]
    public string? Street { get; set; }

    [JsonProperty("city")]
    public string? City { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }

    [JsonProperty("postalcode")]
    public string? PostalCode { get; set; }

    [JsonProperty("address_string")]
    public string? AddressString { get; set; }
}