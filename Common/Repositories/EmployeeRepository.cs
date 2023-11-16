using Common.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HttpClient _httpClient;
        private const string apiToken = "0bf7fb56e6a27cbcadc402fc2fce8e3aa9ac2b40d4190698eb4e8df9284e2023";
        private const string apiUrl = "https://gorest.co.in/public/v2/users";

        public EmployeeRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var uri = $"{apiUrl}";

            var response = await _httpClient.GetStringAsync(uri);
            return JsonSerializer.Deserialize<List<Employee>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Employee>> GetEmployeesByNameAsync(string name)
        {
            var uri = $"{apiUrl}?name={Uri.EscapeDataString(name)}";

            var response = await _httpClient.GetStringAsync(uri);
            return JsonSerializer.Deserialize<List<Employee>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var uri = $"{apiUrl}/{employeeId}";

            var response = await _httpClient.GetStringAsync(uri);
            return JsonSerializer.Deserialize<Employee>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            var uri = $"{apiUrl}";

            var employeeJson = JsonSerializer.Serialize(employee, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditEmployeeAsync(Employee employee)
        {
            var uri = $"{apiUrl}/{employee.Id}";

            var employeeJson = JsonSerializer.Serialize(employee);
            var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var uri = $"{apiUrl}/{employeeId}";

            var response = await _httpClient.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }
    }
}
