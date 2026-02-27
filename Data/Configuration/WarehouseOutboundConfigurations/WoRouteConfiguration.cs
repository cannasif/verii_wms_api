using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WoRouteConfiguration : BaseRouteEntityConfiguration<WoRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WoRoute> builder)
        {
            builder.ToTable("RII_WO_ROUTE");

            builder.Property(x => x.ImportLineId).IsRequired();
            

            builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_WoRoute_ImportLineId");
            builder.HasIndex(x => x.SerialNo).HasDatabaseName("IX_WoRoute_SerialNo");
            builder.HasIndex(x => x.SourceWarehouse).HasDatabaseName("IX_WoRoute_SourceWarehouse");
            builder.HasIndex(x => x.TargetWarehouse).HasDatabaseName("IX_WoRoute_TargetWarehouse");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WoRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}