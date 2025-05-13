using BookingSite.ViewModels.Interface;
using Newtonsoft.Json;

namespace BookingSite.ViewModels;

public class RootHotelImageObject : IRootObject<HotelImageViewModel>
{
    [JsonProperty("data")]
    public List<HotelImageViewModel>? Data { get; set; }
}

public class HotelImageViewModel
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("is_blessed")]
    public bool IsBlessed { get; set; }

    [JsonProperty("published_date")]
    public string? PublishedDate { get; set; }

    [JsonProperty("images")]
    public ImageSizes? Images { get; set; }

    // Optional extra properties if needed
    [JsonProperty("caption")]
    public string? Caption { get; set; }

    [JsonProperty("album")]
    public string? Album { get; set; }

    [JsonProperty("source")]
    public SourceInfo? Source { get; set; }

    [JsonProperty("user")]
    public UserInfo? User { get; set; }
}

public class ImageSizes
{
    [JsonProperty("thumbnail")]
    public ImageDetails? Thumbnail { get; set; }

    [JsonProperty("small")]
    public ImageDetails? Small { get; set; }

    [JsonProperty("medium")]
    public ImageDetails? Medium { get; set; }

    [JsonProperty("large")]
    public ImageDetails? Large { get; set; }

    [JsonProperty("original")]
    public ImageDetails? Original { get; set; }
}

public class ImageDetails
{
    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }
}

public class SourceInfo
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("localized_name")]
    public string? LocalizedName { get; set; }
}

public class UserInfo
{
    [JsonProperty("username")]
    public string? Username { get; set; }
}
