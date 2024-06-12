

namespace Crayon.API.Models.Domain
{
    public class Customer : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();

    }
}
