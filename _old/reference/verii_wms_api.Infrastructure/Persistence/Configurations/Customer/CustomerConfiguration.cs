using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("RII_CUSTOMER");

            builder.Property(x => x.CustomerCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.TaxOffice)
                .HasMaxLength(100);

            builder.Property(x => x.TaxNumber)
                .HasMaxLength(50);

            builder.Property(x => x.TcknNumber)
                .HasMaxLength(11);

            builder.Property(x => x.SalesRepCode)
                .HasMaxLength(50);

            builder.Property(x => x.GroupCode)
                .HasMaxLength(50);

            builder.Property(x => x.CreditLimit)
                .HasColumnType("decimal(18,6)");

            builder.Property(x => x.Email)
                .HasMaxLength(100);

            builder.Property(x => x.Website)
                .HasMaxLength(100);

            builder.Property(x => x.Phone1)
                .HasMaxLength(100);

            builder.Property(x => x.Phone2)
                .HasMaxLength(100);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.City)
                .HasMaxLength(100);

            builder.Property(x => x.District)
                .HasMaxLength(100);

            builder.Property(x => x.CountryCode)
                .HasMaxLength(20);

            builder.Property(x => x.IsErpIntegrated)
                .HasDefaultValue(true);

            builder.Property(x => x.ErpIntegrationNumber)
                .HasMaxLength(50);

            builder.HasIndex(x => x.CustomerCode)
                .IsUnique()
                .HasDatabaseName("IX_RII_CUSTOMER_CustomerCode");

            builder.HasIndex(x => x.CustomerName)
                .HasDatabaseName("IX_RII_CUSTOMER_CustomerName");

            builder.HasIndex(x => x.TaxNumber)
                .HasDatabaseName("IX_RII_CUSTOMER_TaxNumber");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_RII_CUSTOMER_IsDeleted");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
