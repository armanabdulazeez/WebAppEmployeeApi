using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Services.RepServices
{
    public class AddressApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string GetAddressByEmpId = "api/EmployeesAddress/GetAddressesByEmpId/{0}";

        public AddressApiClient(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            string? baseUrl = configuration["AddressApi:BaseUrl"];
            string? apiKey = configuration["AddressApi:ApiKey"];

            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

            _httpClient.DefaultRequestHeaders.Add("api-key", apiKey );
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<EmployeeAddressModel>?> GetAddressByEmpIdAsync(int empId)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token)){
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }
            var response = await _httpClient.GetAsync(string.Format(GetAddressByEmpId,empId));

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EmployeeAddressModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
