using Api.Models;

namespace Api.Repository
{
    public interface IDependentRepository
    {
        Task<Dependent?> GetDependentByIdAsync(int id);
        Task<List<Dependent>> GetAllDependentsAsync();
    }

}
