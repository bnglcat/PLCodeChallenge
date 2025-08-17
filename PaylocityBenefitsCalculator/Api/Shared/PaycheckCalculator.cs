
using Api.Models;

namespace Api.Shared
{
    public class PaycheckCalculator : IPaycheckCalculator
    {
        private readonly PayrollConfiguration _configuration;

        public PaycheckCalculator(PayrollConfiguration configuration)
        {
            _configuration = configuration;
        }

        public decimal CalculateDependentDeductions(ICollection<Dependent> dependents)
        {
            // Base dependent cost is $600 per dependent
            var dependentCosts = dependents.Count * _configuration!.DependantCostPerMonth * 12;

            // Additional costs for dependents over 50 years old
            dependentCosts += dependents.Count(d => d.DateOfBirth < DateTime.Now.AddYears(-_configuration!.AdditionalCostAgeThreashold)) * _configuration!.AdditionaAgeCostPerMonth * 12m;

            return Math.Round( dependentCosts / _configuration.PayPeriodsPerYear, 2);
        }

        public decimal CalculateEmployeeDeductions(decimal salary)
        {
            var employeeDeduction = _configuration.EmployeeCostPerMonth * 12;

            if (salary > _configuration!.AdditionalCostSalaryThreshold)
            {
                // Additional deduction for high earners
                // At this point I am using a flat 2% of the salary as the additional cost.  The business requirements
                // did not specify that this is a per month cost, so I am assuming it is a one time annual cost that will 
                // be divided out over the pay periods. This would be a good question to clarify with the business.
                employeeDeduction += salary * _configuration!.AdditionalEmployeeSalaryCostPercentage;
            }

            return Math.Round(employeeDeduction / _configuration.PayPeriodsPerYear, 2);
        }

        public decimal CalculateGrossPay(decimal salary)
        {
            return Math.Round(salary / _configuration.PayPeriodsPerYear, 2);
        }

        public decimal CalculateNetPay(decimal grossPay, decimal dependentDeductions, decimal employeeDeductions)
        {
            return Math.Round(grossPay - dependentDeductions - employeeDeductions, 2);
        }
    }
}
