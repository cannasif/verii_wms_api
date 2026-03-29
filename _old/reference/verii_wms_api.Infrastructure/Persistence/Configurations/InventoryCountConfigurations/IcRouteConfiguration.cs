using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class IcRouteConfiguration : BaseEntityConfiguration<IcRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<IcRoute> builder)
        {
            builder.ToTable("RII_IC_ROUTE");

            builder.Property(x => x.ImportLineId).IsRequired();
            builder.Property(x => x.Barcode).HasMaxLength(50).IsRequired();
            builder.Property(x => x.OldQuantity).HasColumnType("decimal(18,6)").IsRequired();
            builder.Property(x => x.NewQuantity).HasColumnType("decimal(18,6)").IsRequired();
            builder.Property(x => x.SerialNo).HasMaxLength(50);
            builder.Property(x => x.SerialNo2).HasMaxLength(50);
            builder.Property(x => x.OldWarehouse);
            builder.Property(x => x.NewWarehouse);
            builder.Property(x => x.OldCellCode).HasMaxLength(20);
            builder.Property(x => x.NewCellCode).HasMaxLength(20);
            builder.Property(x => x.Description).HasMaxLength(100);

            builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_IcRoute_ImportLineId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_IcRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany(x => x.Routes)
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}