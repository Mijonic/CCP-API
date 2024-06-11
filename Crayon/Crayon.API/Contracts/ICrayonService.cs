using Crayon.API.Models.Dto;

namespace Crayon.API.Contracts
{
    public interface ICrayonService
    {
        Task<AccountsPage> GetAccounts(Guid userId, int pageNumber);
    }
}
