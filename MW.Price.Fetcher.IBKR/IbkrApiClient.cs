using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MW.Price.Fetcher.IBKR
{
    public class IbkrApiClient
    {
        private const string BaseApiUrl = "https://localhost:5000/v1/api/";
        private const string JsonMediaType = "application/json";

        private readonly HttpClient _http;

        public IbkrApiClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true // allow self-signed
            };

            _http = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseApiUrl)
            };
        }

        public async Task<string?> GetAsync(string endpoint)
        {
            try
            {
                var response = await _http.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GET {endpoint} failed: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> PostAsync(string endpoint, object body)
        {
            try
            {
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, JsonMediaType);

                var response = await _http.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ POST {endpoint} failed: {ex.Message}");
                return null;
            }
        }
    }
}
