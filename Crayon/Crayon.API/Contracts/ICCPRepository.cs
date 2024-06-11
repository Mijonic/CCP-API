using Crayon.API.Models.Domain;

namespace Crayon.API.Contracts
{
    public interface ICCPRepository
    {
        Task<IEnumerable<ServiceCCPModel>> GetPage(int pageNumber, int pageSize);
        Task<OrderSoftwareCCPModel> OrderSoftware(Guid id, Guid accountId);
    }
}
