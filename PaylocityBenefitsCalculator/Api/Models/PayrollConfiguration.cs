namespace Api.Models
{
    public class PayrollConfiguration
    {
        public int Id { get; set; }
       
        public int ClientId { get; set; }

        public int PayPeriodsPerYear { get; set; }

        public decimal DependantCostPerMonth { get; set; }

        public decimal EmployeeCostPerMonth { get; set; }

        public decimal AdditionalCostSalaryThreshold { get; set; }

        public decimal AdditionalEmployeeSalaryCostPercentage { get; set; }

        public int AdditionalCostAgeThreashold { get; set; }

        public decimal AdditionaAgeCostPerMonth { get; set; }
    }
}
