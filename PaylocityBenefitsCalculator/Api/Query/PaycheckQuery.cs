using Api.Dtos.Paycheck;
using Api.Models;
using Api.Repository;
using Api.Shared;

namespace Api.Query
{
    public interface IPaycheckQuery
    {
        Task<GetPaycheckDto> GetAllPaychecksByEmployeeIdAsync(int employeeId);
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

        public async Task<GetPaycheckDto> GetAllPaychecksByEmployeeIdAsync(int employeeId)
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

            // Calculate per period totals
            var dependentDeductionsPerPeriod = payrollCalculator.CalculateDependentDeductionsPerPeriod(employee.Dependents);
            var employeeDeductionsPerPeriod = payrollCalculator.CalculateEmployeeDeductionsPerPeriod(employee.Salary);
            var perPeriodGrossPay = payrollCalculator.CalculateGrossPayPerPeriod(employee.Salary);

            int periods = _payrollConfiguration.PayPeriodsPerYear;

            // Distribute evenly across pay periods
            var dependentDeductionsList = payrollCalculator.CalculateEvenDistribution(dependentDeductionsPerPeriod, periods);
            var employeeDeductionsList = payrollCalculator.CalculateEvenDistribution(employeeDeductionsPerPeriod, periods);
            var grossPayList = payrollCalculator.CalculateEvenDistribution(perPeriodGrossPay, periods);

            var payCheckDto = new GetPaycheckDto
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                Salary = employee.Salary
            };

            for (int i = 0; i < _payrollConfiguration.PayPeriodsPerYear; i++)
            {
                var paycheck = new Paycheck
                {
                    PayPeriod = i + 1,
                    DependentDeductions = dependentDeductionsList[i],
                    EmployeeDeductions = employeeDeductionsList[i],
                    GrossPay = grossPayList[i]
                };

                payCheckDto.Paychecks.Add(paycheck);
            }

            return await Task.FromResult(payCheckDto);
        }
    }
}
