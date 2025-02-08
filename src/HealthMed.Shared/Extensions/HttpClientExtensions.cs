namespace HealthMed.Shared.Extensions
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static async Task<T?> GetDataFromJsonAsync<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

}
