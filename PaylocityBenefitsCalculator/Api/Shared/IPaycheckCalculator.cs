using Api.Models;

namespace Api.Shared
{
    public interface IPaycheckCalculator
    {
        decimal CalculateDependentDeductions(ICollection<Dependent> dependents);
        decimal CalculateEmployeeDeductions(decimal salary);

        decimal CalculateGrossPay(decimal salary);
        decimal CalculateNetPay(decimal grossPay, decimal dependentDeductions, decimal employeeDeductions);
    }
}
