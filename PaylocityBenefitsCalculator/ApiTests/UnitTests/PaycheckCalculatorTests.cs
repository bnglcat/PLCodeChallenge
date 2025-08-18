using Api.Models;
using Api.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTests.UnitTests;

public class PaycheckCalculatorTests
{
    private PayrollConfiguration GetDefaultConfig(int periods = 26) => new PayrollConfiguration
    {
        Id = 1,
        ClientId = 1,
        PayPeriodsPerYear = periods,
        DependantCostPerMonth = 600m,
        EmployeeCostPerMonth = 1000m,
        AdditionalCostSalaryThreshold = 80000m,
        AdditionalEmployeeSalaryCostPercentage = 0.02m,
        AdditionalCostAgeThreashold = 50,
        AdditionaAgeCostPerMonth = 200m
    };

    [Fact]
    public void CalculateDependentDeductionsWhenNoDependents_ShouldReturnZero()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var result = paycheckCalculator.CalculateDependentDeductionsPerPeriod(new List<Dependent>());

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateDependentDeductions_ShouldCalculateBaseCost()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var dependents = new List<Dependent>
        {
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold-10)) },
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold-15)) }
        };

        var expected = (dependents.Count * config.DependantCostPerMonth * 12m) / config.PayPeriodsPerYear;
        var result = paycheckCalculator.CalculateDependentDeductionsPerPeriod(dependents);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateDependentDeductions_ShouldAddAgeCostForOlderDependents()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var dependents = new List<Dependent>
        {
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold+10)) }, // over threshold
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold-10)) }  // under threshold
        };

        var baseCost = dependents.Count * config.DependantCostPerMonth * 12m;
        var ageCount = dependents.Count(x => x.DateOfBirth < DateTime.Now.AddYears(-config.AdditionalCostAgeThreashold));
        var ageCost = ageCount * config.AdditionaAgeCostPerMonth * 12m;
        var expected = (baseCost + ageCost) / config.PayPeriodsPerYear;
        var result = paycheckCalculator.CalculateDependentDeductionsPerPeriod(dependents);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateEmployeeDeductions_ShouldNotAddExtra_WhenSalaryBelowThreshold()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var salary = 70000m;
        var expected = config.EmployeeCostPerMonth * 12m / config.PayPeriodsPerYear;
        var result = paycheckCalculator.CalculateEmployeeDeductionsPerPeriod(salary);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateEmployeeDeductions_ShouldAddExtra_WhenSalaryAboveThreshold()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var salary = 100000m;
        var baseDeduction = config.EmployeeCostPerMonth * 12m;
        var extra = salary * config.AdditionalEmployeeSalaryCostPercentage;
        var expected = (baseDeduction + extra) / config.PayPeriodsPerYear;
        var result = paycheckCalculator.CalculateEmployeeDeductionsPerPeriod(salary);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateGrossPay_ShouldDivideSalaryByPayPeriods()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var salary = 52000m;
        var expected = Math.Round(salary / config.PayPeriodsPerYear, 2);
        var result = paycheckCalculator.CalculateGrossPayPerPeriod(salary);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateNetPay_ShouldSubtractDeductions()
    {
        var config = GetDefaultConfig();
        var paycheckCalculator = new PaycheckCalculator(config);

        var gross = 2000m;
        var dependentDeduction = 300m;
        var employeeDeduction = 400m;

        var expected = Math.Round(gross - dependentDeduction - employeeDeduction, 2);
        var result = paycheckCalculator.CalculateNetPayPerPeriod(gross, dependentDeduction, employeeDeduction);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EvenDistribution_ExactDivision_ShouldDistributeEqually()
    {
        var config = GetDefaultConfig(4);
        var paycheckCalculator = new PaycheckCalculator(config);
        decimal total = 400m;
        int periods = 4;

        var result = paycheckCalculator.CalculateEvenDistribution(total / periods, periods);

        Assert.All(result, v => Assert.Equal(100m, v));
        Assert.Equal(total, Math.Round(result.Sum(), 2));
    }

    [Fact]
    public void EvenDistribution_WithRemainder_ShouldDistributeCents()
    {
        var config = GetDefaultConfig(3);
        var paycheckCalculator = new PaycheckCalculator(config);
        decimal total = 100m;
        int periods = 3;

        var result = paycheckCalculator.CalculateEvenDistribution(total / periods, periods);

        // Each period: 33.33, 33.33, 33.34 (sum to 100.00)
        Assert.Equal(3, result.Count);
        Assert.Equal(100.00m, Math.Round(result.Sum(), 2));
        Assert.Contains(result, v => v == 33.34m);
        Assert.Contains(result, v => v == 33.33m);
    }

    [Fact]
    public void EvenDistribution_ZeroTotal_ShouldReturnAllZeros()
    {
        var config = GetDefaultConfig(5);
        var paycheckCalculator = new PaycheckCalculator(config);
        decimal total = 0m;
        int periods = 5;

        var result = paycheckCalculator.CalculateEvenDistribution(total, periods);

        Assert.All(result, v => Assert.Equal(0m, v));
        Assert.Equal(0m, result.Sum());
    }

    [Fact]
    public void EvenDistribution_OnePeriod_ShouldReturnTotal()
    {
        var config = GetDefaultConfig(1);
        var paycheckCalculator = new PaycheckCalculator(config);
        decimal total = 123.45m;
        int periods = 1;

        var result = paycheckCalculator.CalculateEvenDistribution(total, periods);

        Assert.Single(result);
        Assert.Equal(total, result[0]);
    }
}
