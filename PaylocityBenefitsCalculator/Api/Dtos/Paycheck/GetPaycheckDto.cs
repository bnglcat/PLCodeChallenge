namespace Api.Dtos.Paycheck
{
    public class GetPaycheckDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public ICollection<Paycheck> Paychecks { get; set; } = new List<Paycheck>();
    }

    public class Paycheck
    {
        // A real world scenario would use a DateTime for the pay period, but for simplicity, we use an int here.
        public int PayPeriod { get; set; }
        public decimal DependentDeductions { get; set; }
        public decimal EmployeeDeductions { get; set; }
        public decimal GrossPay { get; set; }
        public decimal NetPay => GrossPay - (DependentDeductions + EmployeeDeductions);
    }
}
