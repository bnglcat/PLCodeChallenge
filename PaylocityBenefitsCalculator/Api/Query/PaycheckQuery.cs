using Api.Dtos.Paycheck;
using Api.Models;
using Api.Repository;
using Api.Shared;

namespace Api.Query
{
    public interface IPaycheckQuery
    {
        Task<GetPaycheckDto> GetPaycheckByEmployeeIdAsync(int employeeId);
    }

    public class MockClient
    {
        public int Id { get; set; }
    }

    public class PaycheckQuery : IPaycheckQuery
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollConfigurationRepository _payrollConfigurationRepository;

        // Pretending that we were able to pull the client information from the request at some point so we could have an id
        // to get the payroll configuration for the client.  Changing this to id 2 will cause all unit tests to fail in their 
        // current state.
        private readonly MockClient _mockClient = new MockClient { Id = 1 };

        private PayrollConfiguration? _payrollConfiguration;

        public PaycheckQuery(IEmployeeRepository employeeRepository, IPayrollConfigurationRepository payrollConfigurationRepository)
        {
            _employeeRepository = employeeRepository;
            _payrollConfigurationRepository = payrollConfigurationRepository;
        }

        public async Task<GetPaycheckDto> GetPaycheckByEmployeeIdAsync(int employeeId)
        {
            _payrollConfiguration = await _payrollConfigurationRepository.GetClientPayrollConfiguration(_mockClient.Id);

            if (_payrollConfiguration == null)
            {
                throw new KeyNotFoundException($"Payroll configuration for client ID {_mockClient.Id} not found.");
            }

            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found.");
            }

            var payrollCalculator = PaycheckCalculatorFactory.CreatePaycheckCalculator(_payrollConfiguration);

            var dependentYearlyDeductions = payrollCalculator.CalculateDependentDeductions(employee.Dependents);
            var employeeYearlyDeductions = payrollCalculator.CalculateEmployeeDeductions(employee.Salary);

            var payCheck = new GetPaycheckDto
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                Salary = employee.Salary,
                GrossPay = payrollCalculator.CalculateGrossPay(employee.Salary),
                DependentDeductions = dependentYearlyDeductions,
                EmployeeDeductions = employeeYearlyDeductions
            };

            BuildEmployeePaycheck(payCheck, dependentYearlyDeductions, employeeYearlyDeductions);

            return await Task.FromResult(payCheck);
        }

        private void BuildEmployeePaycheck(GetPaycheckDto payCheck, decimal dependentYearlyDeductions, decimal employeeYearlyDeductions)
        {
            payCheck.GrossPay = Math.Round(payCheck.Salary / _payrollConfiguration!.PayPeriodsPerYear, 2);
            payCheck.DependentDeductions = Math.Round(dependentYearlyDeductions / _payrollConfiguration!.PayPeriodsPerYear, 2);
            payCheck.EmployeeDeductions = Math.Round(employeeYearlyDeductions / _payrollConfiguration!.PayPeriodsPerYear, 2);
        }
        
    }


}
