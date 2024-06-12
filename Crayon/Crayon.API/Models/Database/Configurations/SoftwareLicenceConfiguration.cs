using Crayon.API.Models.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Crayon.API.Models.Enums;

namespace Crayon.API.Models.Database.Configurations
{
    public class SoftwareLicenceConfiguration : IEntityTypeConfiguration<SoftwareLicence>
    {
        public void Configure(EntityTypeBuilder<SoftwareLicence> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Quantity)
                .IsRequired(true);

            builder.Property(i => i.State)
                .HasDefaultValue(SoftwareLicenceState.Active);

            builder.Property(i => i.SubscriptionEndDate)
                .IsRequired(true);


            builder.HasOne(i => i.Account)
               .WithMany(p => p.Licences)
               .HasForeignKey(i => i.AccountId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}


