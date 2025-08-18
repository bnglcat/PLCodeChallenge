using Api.Dtos.Paycheck;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public class PaycheckIntegrationTests : IntegrationTest
    {
        [Fact]
        public async Task WhenAskedForPaycheckForInvalidEmployee_ShouldReturnNotFound()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/{int.MinValue}");
            await response.ShouldReturn(HttpStatusCode.NotFound);
        }

        // The following tests are all working against the currently hard coded list of dependents and employees in the mock dependents 
        // employee respositories.  In a real application, I would build out a full set of mock repositories for the tests so that 
        // we could control the data and ensure that the tests are isolated from each other.  However, in the interest of time,
        // I have just used the existing repositories and hard coded data.  

        [Fact]
        public async Task WhenAskedForPaycheckForEmployeeNoDependants_ShouldReturnZeroDependentDeductionValue()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/1");

            await response.ShouldReturn(HttpStatusCode.OK, _noDependentDeductionScenario);
        }

        [Fact]
        public async Task WhenAskedForPaycheckForEmployeeOneDependant_ShouldReturnCorrectDependentDeductionValue()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/2");

            await response.ShouldReturn(HttpStatusCode.OK, _dependentDeductions);
        }

        [Fact]
        public async Task WhenAskedForPaycheckForEmployeeOneDependant_ShouldReturnCorrectEmployeeDeductionValue()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/3");

            await response.ShouldReturn(HttpStatusCode.OK, _dependentDeductionsWithEmployeeExtraFee);
        }

        #region Scenarios

        private GetPaycheckDto _noDependentDeductionScenario = new GetPaycheckDto
        {
            EmployeeId = 1,
            EmployeeName = "LeBron James",
            Salary = 75420.99m,
            Paychecks =
                [
                    new Paycheck { PayPeriod = 1, DependentDeductions = 0.00m, EmployeeDeductions = 461.53m, GrossPay = 2900.80m},
                new Paycheck { PayPeriod = 2, DependentDeductions = 0.00m, EmployeeDeductions = 461.53m, GrossPay = 2900.80m},
                new Paycheck { PayPeriod = 3, DependentDeductions = 0.00m, EmployeeDeductions = 461.53m, GrossPay = 2900.80m },
                new Paycheck { PayPeriod = 4, DependentDeductions = 0.00m, EmployeeDeductions = 461.53m, GrossPay = 2900.80m },
                new Paycheck { PayPeriod = 5, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.80m },
                new Paycheck { PayPeriod = 6, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.80m },
                new Paycheck { PayPeriod = 7, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.80m },
                new Paycheck { PayPeriod = 8, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 9, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 10, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 11, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 12, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 13, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 14, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 15, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 16, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 17, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 18, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 19, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 20, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 21, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 22, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 23, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 24, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 25, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m },
                new Paycheck { PayPeriod = 26, DependentDeductions = 0.00m, EmployeeDeductions = 461.54m, GrossPay = 2900.81m }
                ]
        };

        private GetPaycheckDto _dependentDeductions = new GetPaycheckDto
        {
            EmployeeId = 2,
            EmployeeName = "Ja Morant",
            Salary = 92365.22m,
            Paychecks =
            [
                new Paycheck { PayPeriod = 1, DependentDeductions = 830.76m, EmployeeDeductions = 532.58m, GrossPay = 3552.50m},
                new Paycheck { PayPeriod = 2, DependentDeductions = 830.76m, EmployeeDeductions = 532.58m, GrossPay = 3552.50m},
                new Paycheck { PayPeriod = 3, DependentDeductions = 830.77m, EmployeeDeductions = 532.58m, GrossPay = 3552.50m},
                new Paycheck { PayPeriod = 4, DependentDeductions = 830.77m, EmployeeDeductions = 532.58m, GrossPay = 3552.50m},
                new Paycheck { PayPeriod = 5, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 6, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 7, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 8, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 9, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 10, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 11, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 12, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 13, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 14, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 15, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 16, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 17, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 18, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 19, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 20, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 21, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 22, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 23, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 24, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 25, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m},
                new Paycheck { PayPeriod = 26, DependentDeductions = 830.77m, EmployeeDeductions = 532.59m, GrossPay = 3552.51m}
            ]
        };

        private GetPaycheckDto _dependentDeductionsWithEmployeeExtraFee = new GetPaycheckDto
        {
            EmployeeId = 3,
            EmployeeName = "Michael Jordan",
            Salary = 143211.12m,
            Paychecks =
            [
                new Paycheck { PayPeriod = 1, DependentDeductions = 369.24m, EmployeeDeductions = 571.71m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 2, DependentDeductions = 369.24m, EmployeeDeductions = 571.71m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 3, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 4, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 5, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 6, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 7, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 8, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 9, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 10, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 11, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 12, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 13, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 14, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 15, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 16, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 17, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 18, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 19, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 20, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 21, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 22, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 23, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 24, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 25, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m },
                new Paycheck { PayPeriod = 26, DependentDeductions = 369.23m, EmployeeDeductions = 571.70m, GrossPay = 5508.12m }
            ]
        };
    }
    #endregion
}
