using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WiRouteConfiguration : BaseRouteEntityConfiguration<WiRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WiRoute> builder)
        {
            builder.ToTable("RII_WI_ROUTE");

            builder.Property(x => x.ImportLineId).IsRequired();
            

            builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_WiRoute_ImportLineId");
            builder.HasIndex(x => x.SerialNo).HasDatabaseName("IX_WiRoute_SerialNo");
            builder.HasIndex(x => x.SourceWarehouse).HasDatabaseName("IX_WiRoute_SourceWarehouse");
            builder.HasIndex(x => x.TargetWarehouse).HasDatabaseName("IX_WiRoute_TargetWarehouse");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WiRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}