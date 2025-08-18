
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

        public decimal CalculateDependentDeductionsPerPeriod(ICollection<Dependent> dependents)
        {
            var dependentCosts = dependents.Count * _configuration!.DependantCostPerMonth * 12;

            dependentCosts += dependents.Count(d => d.DateOfBirth < DateTime.Now.AddYears(-_configuration!.AdditionalCostAgeThreashold)) * _configuration!.AdditionaAgeCostPerMonth * 12;

            return dependentCosts / _configuration.PayPeriodsPerYear;
        }

        public decimal CalculateEmployeeDeductionsPerPeriod(decimal salary)
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

            return employeeDeduction / _configuration.PayPeriodsPerYear;
        }

        public decimal CalculateGrossPayPerPeriod(decimal salary)
        {
            return salary / _configuration.PayPeriodsPerYear;
        }

        public decimal CalculateNetPayPerPeriod(decimal grossPay, decimal dependentDeductions, decimal employeeDeductions)
        {
            return grossPay - dependentDeductions - employeeDeductions;
        }

        public List<decimal> CalculateEvenDistribution(decimal monthlyTotal, int periods)
        {
            var perPeriodUnrounded = monthlyTotal;
            var perPeriodRounded = Math.Round(perPeriodUnrounded, 2, MidpointRounding.AwayFromZero);

            var anualTotal = perPeriodUnrounded * periods;

            var values = Enumerable.Repeat(perPeriodRounded, periods).ToList();
            var totalRounded = perPeriodRounded * periods;
            var remainder = Math.Round(anualTotal - totalRounded, 2);

            int centsToDistribute = (int)(remainder * 100);
            for (int i = 0; i < Math.Abs(centsToDistribute); i++)
            {
                if (centsToDistribute > 0)
                    values[i] += 0.01m;
                else if (centsToDistribute < 0)
                    values[i] -= 0.01m;
            }
            return values;
        }
    }
}
