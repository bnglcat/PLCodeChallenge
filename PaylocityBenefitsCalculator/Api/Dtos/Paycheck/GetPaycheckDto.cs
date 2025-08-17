namespace Api.Dtos.Paycheck
{
    public class GetPaycheckDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal GrossPay { get; set; }
        public decimal DependentDeductions { get; set; }

        public decimal EmployeeDeductions { get; set; }
        public decimal NetPay => GrossPay - (DependentDeductions + EmployeeDeductions);

        public decimal Salary { get; set; }
    }
}
