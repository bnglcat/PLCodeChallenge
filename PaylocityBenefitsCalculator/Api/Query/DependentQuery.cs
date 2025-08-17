using Api.Models;
using Api.Repository;

namespace Api.Query
{
    public interface IDependentQuery
    {
        Task<Dependent?> GetDependentByIdAsync(int id);

        Task<List<Dependent>> GetAllDependentsAsync();
    }

    public class DependentQuery : IDependentQuery
    {
        private readonly IDependentRepository _dependentRepository;

        public DependentQuery(IDependentRepository DependentRepository)
        { 
            _dependentRepository = DependentRepository;
        }

        public async Task<List<Dependent>> GetAllDependentsAsync()
        {
            return await _dependentRepository.GetAllDependentsAsync();
        }

        public async Task<Dependent?> GetDependentByIdAsync(int id)
        {
            return await _dependentRepository.GetDependentByIdAsync(id);
        }
    }
}
