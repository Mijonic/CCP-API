namespace Crayon.API.Models.Dto.Input
{
    public class UpdateQuantityInputDto
    {
        public Guid LicenceId { get; set; } 
        public int Quantity { get; set; }

        public void Validate()
        {
            if (Guid.Empty == LicenceId) throw new ArgumentException();

            if (Quantity <= 0) throw new ArgumentException();
        }
    }
}
