using Crayon.API.Models.Domain;
using Crayon.API.Models.Dto;
using Crayon.API.Models.Dto.Input;

namespace Crayon.API.Contracts
{
    public interface ICrayonService
    {
        Task<AccountsPage> GetAccounts(Guid userId, int pageNumber);
        Task<IEnumerable<AvailableSoftwareLicenceDto>> GetAvailableServices(int pageNumber);
        Task<IEnumerable<SoftwareLicenceDto>> GetLicencesForAccount(Guid accountId, int pageNumber);
        Task<OrderSotwareDto> OrderSoftware(OrderSoftwareInputDto orderSoftwareInput);
        Task<SoftwareLicenceDto> ModifyQuantity(UpdateQuantityInputDto updateQuantityInput);
        Task<SoftwareLicenceDto> CancelLicence(Guid id);
        Task<SoftwareLicenceDto> ExtendLicence(Guid id, DateTimeOffset extendedDate);

    }
}
