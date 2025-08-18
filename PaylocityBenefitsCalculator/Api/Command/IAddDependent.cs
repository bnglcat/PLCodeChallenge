using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Command
{
    public interface IAddDependent
    {
        Task<Dependent> AddDependentAsync(AddDependentDto dependent);
    }
}
