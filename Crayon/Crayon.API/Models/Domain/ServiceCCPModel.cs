using Crayon.API.Models.Enums;

namespace Crayon.API.Models.Domain
{
    public class ServiceCCPModel : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
