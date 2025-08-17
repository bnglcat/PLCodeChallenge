using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Command
{
    public interface IAddDependent
    {
        Task<Dependent> AddDependentAsync(AddDependentDto dependent);
        //Task<bool> UpdateDependentAsync(int id, UpdateDependentDto dependentDto);
        //Task<bool> DeleteDependentAsync(int id);
    }
}
