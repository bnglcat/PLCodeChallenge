using Api.Dtos.Dependent;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
    [Fact]
    //task: make test pass
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        var dependents = new List<GetDependentDto>
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }


    // The following tests are all working against the currently hard coded list of dependents and employees in the mock dependents 
    // employee respositories.  In a real application, I would build out a full set of mock repositories for the tests so that 
    // we could control the data and ensure that the tests are isolated from each other.  However, in the interest of time,
    // I have just used the existing repositories and hard coded data.  
    [Fact]
    public async Task WhenAddingADependentWithRelationshipNone_ShouldReturnBadRequest()
    {
        const int depenentCount = 4;

        var newDependent = new AddDependentDto
        {
            FirstName = "Test",
            LastName = "Dependent",
            Relationship = Relationship.None,
            DateOfBirth = new DateTime(1990, 1, 1),
            EmployeeId = 1
        };

        var content = JsonContent.Create(newDependent);

        var response = await HttpClient.PostAsync("/api/v1/dependents", content);

        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task WhenAddingADependentWithRelationshipChild_ShouldReturnOk()
    {
        var depenentCount = 4;

        var newDependent = new AddDependentDto
        {
            FirstName = "Test",
            LastName = "Dependent",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(1990, 1, 1),
            EmployeeId = 1
        };

        var content = JsonContent.Create(newDependent);

        var response = await HttpClient.PostAsync("/api/v1/dependents", content);

        newDependent.Id = depenentCount + 1;

        await response.ShouldReturn(HttpStatusCode.OK, newDependent);
    }

    [Fact]
    public async Task WhenAddingADependentWithRelationshipSpouseWithNoSpouse_ShouldReturnOk()
    {
        var depenentCount = 4;

        var newDependent = new AddDependentDto
        {
            FirstName = "Test",
            LastName = "Dependent",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1990, 1, 1),
            EmployeeId = 1
        };

        var content = JsonContent.Create(newDependent);

        var response = await HttpClient.PostAsync("/api/v1/dependents", content);

        newDependent.Id = depenentCount + 1;

        await response.ShouldReturn(HttpStatusCode.OK, newDependent);
    }

    [Fact]
    public async Task WhenAddingADependentWithRelationshipSpouseWithExistingSpouse_ShouldReturnBadRequest()
    {
        var depenentCount = 4;

        var newDependent = new AddDependentDto
        {
            FirstName = "Test",
            LastName = "Dependent",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1990, 1, 1),
            EmployeeId = 2
        };

        var content = JsonContent.Create(newDependent);

        var response = await HttpClient.PostAsync("/api/v1/dependents", content);

        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }
}

