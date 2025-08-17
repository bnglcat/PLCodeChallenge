using Api.Models;

namespace Api.Repository
{

    public class MockDependentRepository : IDependentRepository
    {
        private readonly List<Dependent> _dependents =
        [
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3),
                EmployeeId = 2
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23),
                EmployeeId = 2
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18),
                EmployeeId = 2
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2),
                EmployeeId = 3
            }
        ];

        public Task<Dependent> AddDependentAsync(Dependent dependent)
        {
            if (dependent == null || dependent.EmployeeId <= 0)
            {
                throw new ArgumentException("Invalid dependent data");
            }
            dependent.Id = _dependents.Count + 1; // Simple ID generation
            
            _dependents.Add(dependent);
            
            return Task.FromResult(dependent);


        }

        public async Task<List<Dependent>> GetAllDependentsAsync() => await Task.FromResult(_dependents);
        public async Task<Dependent?> GetDependentByIdAsync(int id) =>
             await Task.FromResult(_dependents.FirstOrDefault(e => e.Id == id));
    }

}
