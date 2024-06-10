namespace Crayon.API.Models.Domain
{
    public interface IDomainModel<TId>
    {
        TId Id { get; set; }
    }
}
