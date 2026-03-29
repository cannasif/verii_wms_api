using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class StockConfiguration : BaseEntityConfiguration<Stock>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("RII_STOCK");

            builder.Property(x => x.ErpStockCode)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StockName)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.Unit)
                .HasMaxLength(20);

            builder.Property(x => x.UreticiKodu)
                .HasMaxLength(50);

            builder.Property(x => x.GrupKodu)
                .HasMaxLength(50);

            builder.Property(x => x.GrupAdi)
                .HasMaxLength(250);

            builder.Property(x => x.Kod1)
                .HasMaxLength(50);

            builder.Property(x => x.Kod1Adi)
                .HasMaxLength(250);

            builder.Property(x => x.Kod2)
                .HasMaxLength(50);

            builder.Property(x => x.Kod2Adi)
                .HasMaxLength(250);

            builder.Property(x => x.Kod3)
                .HasMaxLength(50);

            builder.Property(x => x.Kod3Adi)
                .HasMaxLength(250);

            builder.Property(x => x.Kod4)
                .HasMaxLength(50);

            builder.Property(x => x.Kod4Adi)
                .HasMaxLength(250);

            builder.Property(x => x.Kod5)
                .HasMaxLength(50);

            builder.Property(x => x.Kod5Adi)
                .HasMaxLength(250);

            builder.HasIndex(x => x.ErpStockCode)
                .IsUnique()
                .HasDatabaseName("IX_RII_STOCK_ErpStockCode");

            builder.HasIndex(x => x.StockName)
                .HasDatabaseName("IX_RII_STOCK_StockName");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_RII_STOCK_IsDeleted");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
