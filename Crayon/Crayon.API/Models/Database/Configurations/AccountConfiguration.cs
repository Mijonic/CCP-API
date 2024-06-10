using Crayon.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crayon.API.Models.Database.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(30);


            builder.HasOne(i => i.Customer)
               .WithMany(p => p.Accounts)
               .HasForeignKey(i => i.CustomerId)
               .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
