using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WtRouteConfiguration : BaseRouteEntityConfiguration<WtRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WtRoute> builder)
        {
            builder.ToTable("RII_WT_ROUTE");

            builder.Property(x => x.ImportLineId)
                .IsRequired();

            builder.Property(x => x.SourceWarehouse);

            builder.Property(x => x.TargetWarehouse);


            // Indexes
            builder.HasIndex(x => x.ImportLineId)
                .HasDatabaseName("IX_WtRoute_ImportLineId");

            builder.HasIndex(x => x.SerialNo)
                .HasDatabaseName("IX_WtRoute_SerialNo");

            builder.HasIndex(x => x.SourceWarehouse)
                .HasDatabaseName("IX_WtRoute_SourceWarehouse");

            builder.HasIndex(x => x.TargetWarehouse)
                .HasDatabaseName("IX_WtRoute_TargetWarehouse");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_WtRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany(x => x.Routes)
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}