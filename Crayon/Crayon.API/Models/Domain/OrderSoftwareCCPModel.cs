namespace Crayon.API.Models.Domain
{
    public class OrderSoftwareCCPModel : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; } 
    }
}
