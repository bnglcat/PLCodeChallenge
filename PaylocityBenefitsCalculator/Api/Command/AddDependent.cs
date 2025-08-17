using Api.Dtos.Dependent;
using Api.Models;
using Api.Repository;

namespace Api.Command
{
    public class AddDependent : IAddDependent
    {
        private readonly IDependentRepository _dependentRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AddDependent(IDependentRepository dependentRepository, IEmployeeRepository employeeRepository)
        {
            _dependentRepository = dependentRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Dependent> AddDependentAsync(AddDependentDto dependent)
        {
            if (dependent == null || dependent.EmployeeId <= 0)
            {
                throw new ArgumentException("Invalid dependent data");
            }

            // Check if the employee exists
            var employee = _employeeRepository.GetEmployeeByIdAsync(dependent.EmployeeId).Result;
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            if (dependent.Relationship == Relationship.None)
            {
                throw new ArgumentException("Relationship cannot be None");
            }

            if (dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner)
            {
                // Check if the employee already has a spouse or domestic partner
                if (employee.Dependents.Any(d => d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner))
                {
                    throw new ArgumentException("Employee already has a spouse or domestic partner. Cannot add more than one.");
                }
            }

            var dependentModel = new Dependent
            {
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                Relationship = dependent.Relationship,
                DateOfBirth = dependent.DateOfBirth,
                EmployeeId = dependent.EmployeeId
            };

            return await _dependentRepository.AddDependentAsync(dependentModel);
        }
    }
}
