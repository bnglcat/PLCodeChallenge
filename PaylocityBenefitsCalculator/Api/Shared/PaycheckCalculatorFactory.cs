using Api.Models;

namespace Api.Shared
{
    public class PaycheckCalculatorFactory
    {
        public static IPaycheckCalculator CreatePaycheckCalculator(PayrollConfiguration configuration)
        {
            return new PaycheckCalculator(configuration);
        }
    }
}
