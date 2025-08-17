using Api.Dtos.Paycheck;
using Api.Models;
using Api.Repository;

namespace Api.Query
{
    public interface IPaycheckQuery
    {
        Task<GetPaycheckDto> GetPaycheckByEmployeeIdAsync(int employeeId);
    }

    public class PaycheckQuery : IPaycheckQuery
    {
        private readonly IEmployeeRepository _employeeRepository;

        public PaycheckQuery(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetPaycheckDto> GetPaycheckByEmployeeIdAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }

            var dependentYearlyDeductions = CalculateDependentDeductions(employee.Dependents);
            var employeeYearlyDeductions = CalculateEmployeeDeductions(employee);

            var payCheck = new GetPaycheckDto
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",                
                Salary = employee.Salary                
            };

            BuildEmployeePaycheck(payCheck, dependentYearlyDeductions, employeeYearlyDeductions);

            return await Task.FromResult(payCheck);
        }

        private void BuildEmployeePaycheck(GetPaycheckDto payCheck,  decimal dependentYearlyDeductions, decimal employeeYearlyDeductions)
        {
            payCheck.GrossPay = Math.Round(payCheck.Salary / 26,2);
            payCheck.DependentDeductions = Math.Round(dependentYearlyDeductions / 26, 2);
            payCheck.EmployeeDeductions = Math.Round(employeeYearlyDeductions / 26, 2);
        }

        private decimal CalculateEmployeeDeductions(Employee employee)
        {
            // Base deduction is $1000
            var employeeDeduction = 1000m * 12;

            if (employee.Salary > 80000m)
            {
                // Additional deduction for high earners
                employeeDeduction += employee.Salary * .02m;
            }

            return Math.Round(employeeDeduction, 2);
        }

        private decimal CalculateDependentDeductions(ICollection<Dependent> dependents)
        {
            // Base dependent cost is $600 per dependent
            var dependentCosts = dependents.Count * 600m * 12;

            // Additional costs for dependents over 50 years old
            dependentCosts += dependents.Count(d => d.DateOfBirth < DateTime.Now.AddYears(-50)) * 200m * 12m;

            return dependentCosts;
        }       
    }


}
