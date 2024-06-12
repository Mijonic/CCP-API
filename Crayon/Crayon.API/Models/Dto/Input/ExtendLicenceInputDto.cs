namespace Crayon.API.Models.Dto.Input
{
    public class ExtendLicenceInputDto
    {
        public Guid LicenceId { get; set; }
        public DateTimeOffset EndDate { get; set; } 
    }
}
