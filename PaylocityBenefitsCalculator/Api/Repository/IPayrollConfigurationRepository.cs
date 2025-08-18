using Api.Models;

namespace Api.Repository
{
    public interface IPayrollConfigurationRepository
    {
        Task<PayrollConfiguration?> GetClientPayrollConfiguration(int clientId);
    }
}
