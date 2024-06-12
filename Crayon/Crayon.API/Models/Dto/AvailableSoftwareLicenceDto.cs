using Crayon.API.Models.Domain;

namespace Crayon.API.Models.Dto
{
    public class AvailableSoftwareLicenceDto : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
