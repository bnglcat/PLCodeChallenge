using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

            var paycheck = new
            {
                employeeId = 1,
                employeeName = "LeBron James",
                grossPay = 2900.81,
                dependentDeductions = 0,
                employeeDeductions = 461.54,
                netPay = 2439.27,
                salary = 75420.99
            };

            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }

        [Fact]
        public async Task WhenAskedForPaycheckForEmployeeOneDependant_ShouldReturnCorrectDependentDeductionValue()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/2");

            var paycheck = new
            {
                employeeId = 2,
                employeeName = "Ja Morant",
                grossPay = 3552.51,
                dependentDeductions = 830.77,
                employeeDeductions = 532.59,
                netPay = 2189.15,
                salary = 92365.22
            };

            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }

        [Fact]
        public async Task WhenAskedForPaycheckForEmployeeOneDependant_ShouldReturnCorrectEmployeeDeductionValue()
        {
            var response = await HttpClient.GetAsync($"/api/v1/paychecks/3");

            var paycheck = new
            {
                employeeId = 3,
                employeeName = "Michael Jordan",
                grossPay = 5508.12,
                dependentDeductions = 369.23,
                employeeDeductions = 571.7,
                netPay = 4567.19,
                salary = 143211.12
            };

            await response.ShouldReturn(HttpStatusCode.OK, paycheck);
        }
    }
}
