namespace Crayon.API.Models.Dto
{
    public class OrderSoftwareInputDto
    {
        public Guid SoftwareId { get; set; }
        public Guid AccountId { get; set; }
        public int Quantity { get; set; }
    }
}
