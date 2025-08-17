using Api.Models;

namespace Api.Repository
{
    public class PayrollConfigurationRepository : IPayrollConfigurationRepository
    {
        private readonly List<PayrollConfiguration> _payrollConfigurations = new List<PayrollConfiguration>
        {
            new PayrollConfiguration
            {
                Id = 1,
                ClientId = 1,
                PayPeriodsPerYear = 26,
                DependantCostPerMonth = 600.00m,
                EmployeeCostPerMonth = 1000.00m,
                AdditionalCostSalaryThreshold = 80000.00m,
                AdditionalEmployeeSalaryCostPercentage = .02m,
                AdditionalCostAgeThreashold = 50,
                AdditionaAgeCostPerMonth = 200.00m
            },
            new PayrollConfiguration
            {
                Id = 2,
                ClientId = 2,
                PayPeriodsPerYear = 24,
                DependantCostPerMonth = 200.00m,
                EmployeeCostPerMonth = 500.00m,
                AdditionalCostSalaryThreshold = 50000.00m,
                AdditionalEmployeeSalaryCostPercentage = .01m,
                AdditionalCostAgeThreashold = 65,
                AdditionaAgeCostPerMonth = 100.00m
            }
        };

        public PayrollConfigurationRepository()
        {
           
        }

        public async Task<PayrollConfiguration?> GetClientPayrollConfiguration(int clientId)
        {
            return await Task.FromResult(_payrollConfigurations.FirstOrDefault(pc => pc.ClientId == clientId));
        }
    }
}
