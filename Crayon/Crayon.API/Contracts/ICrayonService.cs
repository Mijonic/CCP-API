using Crayon.API.Models.Dto;

namespace Crayon.API.Contracts
{
    public interface ICrayonService
    {
        Task<AccountsPage> GetAccounts(Guid userId, int pageNumber);
        Task<IEnumerable<AvailableSoftwareLicenceDto>> GetAvailableServices(int pageNumber);
        Task<OrderSotwareDto> OrderSoftware(OrderSoftwareInputDto orderSoftwareInput);

    }
}
