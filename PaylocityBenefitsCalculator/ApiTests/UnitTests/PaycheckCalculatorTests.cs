using Api.Models;
using Api.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTests.UnitTests;

public class PaycheckCalculatorTests
{
    private PayrollConfiguration GetDefaultConfig() => new PayrollConfiguration
    {
        Id = 1,
        ClientId = 1,
        PayPeriodsPerYear = 26,
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
        var calc = new PaycheckCalculator(config);

        var result = calc.CalculateDependentDeductions(new List<Dependent>());

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateDependentDeductions_ShouldCalculateBaseCost()
    {
        var config = GetDefaultConfig();
        var calc = new PaycheckCalculator(config);

        var dependents = new List<Dependent>
        {
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold-10)) },
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold-15)) }
        };

        var expected = Math.Round((dependents.Count * config.DependantCostPerMonth * 12m) / config.PayPeriodsPerYear, 2);
        var result = calc.CalculateDependentDeductions(dependents);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateDependentDeductions_ShouldAddAgeCostForOlderDependents()
    {
        var config = GetDefaultConfig();
        var calc = new PaycheckCalculator(config);

        var dependents = new List<Dependent>
        {
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold+10)) }, // over threshold
            new Dependent { DateOfBirth = DateTime.Now.AddYears(-(config.AdditionalCostAgeThreashold-10)) }  // under threshold
        };

        var baseCost = dependents.Count * config.DependantCostPerMonth * 12m;
        var ageCount = dependents.Count(x => x.DateOfBirth < DateTime.Now.AddYears(-config.AdditionalCostAgeThreashold));
        var ageCost = ageCount * config.AdditionaAgeCostPerMonth * 12m;
        var expected = Math.Round((baseCost + ageCost) / config.PayPeriodsPerYear, 2);
        var result = calc.CalculateDependentDeductions(dependents);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateEmployeeDeductions_ShouldNotAddExtra_WhenSalaryBelowThreshold()
    {
        var config = GetDefaultConfig();
        var calc = new PaycheckCalculator(config);

        var salary = 70000m;
        var expected = Math.Round(config.EmployeeCostPerMonth * 12m / config.PayPeriodsPerYear, 2);
        var result = calc.CalculateEmployeeDeductions(salary);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateEmployeeDeductions_ShouldAddExtra_WhenSalaryAboveThreshold()
    {
        var config = GetDefaultConfig();
        var calc = new PaycheckCalculator(config);

        var salary = 100000m;
        var baseDeduction = config.EmployeeCostPerMonth * 12m;
        var extra = salary * config.AdditionalEmployeeSalaryCostPercentage;
        var expected = Math.Round((baseDeduction + extra) / config.PayPeriodsPerYear, 2);
        var result = calc.CalculateEmployeeDeductions(salary);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateGrossPay_ShouldDivideSalaryByPayPeriods()
    {
        var config = GetDefaultConfig();
        var calc = new PaycheckCalculator(config);

        var salary = 52000m;
        var expected = Math.Round(salary / config.PayPeriodsPerYear, 2);
        var result = calc.CalculateGrossPay(salary);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateNetPay_ShouldSubtractDeductions()
    {
        var config = GetDefaultConfig();
        var calc = new PaycheckCalculator(config);

        var gross = 2000m;
        var depDed = 300m;
        var empDed = 400m;

        var expected = Math.Round(gross - depDed - empDed, 2);
        var result = calc.CalculateNetPay(gross, depDed, empDed);

        Assert.Equal(expected, result);
    }
}
