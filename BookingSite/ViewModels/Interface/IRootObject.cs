using Newtonsoft.Json;

namespace BookingSite.ViewModels.Interface;

public interface IRootObject<T>
{
    [JsonProperty("data")]
    public List<T> Data { get; set; }
}