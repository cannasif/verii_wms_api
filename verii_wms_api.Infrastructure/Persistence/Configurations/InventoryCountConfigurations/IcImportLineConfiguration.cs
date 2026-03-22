using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class IcImportLineConfiguration : BaseEntityConfiguration<IcImportLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<IcImportLine> builder)
        {
            builder.ToTable("RII_IC_IMPORT_LINE");

            builder.Property(x => x.HeaderId).IsRequired();
            builder.Property(x => x.StockCode).HasMaxLength(35).IsRequired();
            builder.Property(x => x.YapKod).HasMaxLength(35);
            builder.Property(x => x.Description1).HasMaxLength(30);
            builder.Property(x => x.Description2).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(255);

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcImportLine_HeaderId");
            builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_IcImportLine_StockCode");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_IcImportLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Routes)
                .WithOne(x => x.ImportLine)
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}