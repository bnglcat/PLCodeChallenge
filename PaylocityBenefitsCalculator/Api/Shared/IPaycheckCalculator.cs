using Api.Models;

namespace Api.Shared
{
    public interface IPaycheckCalculator
    {
        decimal CalculateDependentDeductionsPerPeriod(ICollection<Dependent> dependents);
        decimal CalculateEmployeeDeductionsPerPeriod(decimal salary);
        List<decimal> CalculateEvenDistribution(decimal monthlyTotal, int periods);
        decimal CalculateGrossPayPerPeriod(decimal salary);
        decimal CalculateNetPayPerPeriod(decimal grossPay, decimal dependentDeductions, decimal employeeDeductions);
    }
}
