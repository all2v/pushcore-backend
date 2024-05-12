using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Astrum.Infrastructure;

public static class HttpClientExtensions
{
    static internal Task<HttpResponseMessage> PostAsJsonAsyncHandmade<T>(
        this HttpClient httpClient, string url, T data, string contentType = "application/json")
    {
        var dataAsString = JsonConvert.SerializeObject(data,
            new JsonSerializerSettings {DateFormatString = "yyyy-MM-ddTHH:mm:ssZ"});
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        return httpClient.PostAsync(url, content);
    }

    public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
    {
        var dataAsString = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(dataAsString);
    }
}