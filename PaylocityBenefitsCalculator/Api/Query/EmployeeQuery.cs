using Api.Models;
using Api.Repository;

namespace Api.Query
{
    public interface IEmployeeQuery
    {
        Task<Employee?> GetEmployeeByIdAsync(int id);

        Task<List<Employee>> GetAllEmployeesAsync();
    }

    public class EmployeeQuery : IEmployeeQuery
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeQuery(IEmployeeRepository employeeRepository)
        { 
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }
    }
}
