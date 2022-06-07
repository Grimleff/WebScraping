using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebScrapingAPI.Model
{
    public class SimpleResponse<T>
    {
        [JsonProperty("success")] public bool Success { get; set; }

        [JsonProperty("result")] public List<T> Result { get; set; }

        [JsonProperty("count")] public int Count { get; set; }

        [JsonProperty("error")] public string Error { get; set; }
    }
}