using Api.Models;

namespace Api.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<List<Employee>> GetAllEmployeesAsync();
    }

}
