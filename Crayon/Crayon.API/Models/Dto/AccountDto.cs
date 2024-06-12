using Crayon.API.Models.Domain;

namespace Crayon.API.Models.Dto
{
    public class AccountDto : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
