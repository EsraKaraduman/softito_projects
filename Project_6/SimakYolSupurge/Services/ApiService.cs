using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SimakYolSupurge.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private void AddAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("JwtToken")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            return default;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            AddAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            AddAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            AddAuthHeader();
            return await _httpClient.DeleteAsync(endpoint);
        }

        public async Task<byte[]?> GetFileAsync(string endpoint)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
    }
}
