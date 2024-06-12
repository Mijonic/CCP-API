using Crayon.API.Models.Enums;

namespace Crayon.API.Models.Domain
{
    public class SoftwareLicence : IDomainModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset SubscriptionEndDate { get; set; }
        public SoftwareLicenceState State { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public void UpdateOrderedSoftwareLicence(int quantity, Guid accountId)
        {
            Quantity = quantity;
            AccountId = accountId;
            State = SoftwareLicenceState.Active;
            SubscriptionEndDate = DateTimeOffset.UtcNow.AddMonths(1);
        }

    }
}
