using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesAsync();

        Task<List<Employee>> GetEmployeesByNameAsync(string name);
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task<bool> AddEmployeeAsync(Employee employee);
        Task<bool> EditEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int employeeId);
    }
}
