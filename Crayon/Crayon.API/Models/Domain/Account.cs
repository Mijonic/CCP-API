namespace Crayon.API.Models.Domain
{
    public class Account : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<SoftwareLicence> Licences { get; set; } = new List<SoftwareLicence>();
    }
}
