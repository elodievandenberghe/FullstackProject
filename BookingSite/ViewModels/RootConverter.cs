using BookingSite.ViewModels.Interface;
using Newtonsoft.Json;

namespace BookingSite.ViewModels;

public class RootConverter<T> : JsonConverter
{
    public override bool CanConvert(Type objectType) => true;    
     
    public override object ReadJson(JsonReader reader,    
        Type objectType, object existingValue, JsonSerializer serializer)    
    {    
        return serializer.Deserialize<T>(reader);    
    }    
     
    public override void WriteJson(JsonWriter writer,    
        object value, JsonSerializer serializer)    
    {    
        serializer.Serialize(writer, value);    
    }     
}