using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SitRouteConfiguration : BaseRouteEntityConfiguration<SitRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SitRoute> builder)
        {
            builder.ToTable("RII_SIT_ROUTE");

            builder.Property(x => x.ImportLineId).IsRequired();
            

            builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_SitRoute_ImportLineId");
            builder.HasIndex(x => x.SerialNo).HasDatabaseName("IX_SitRoute_SerialNo");
            builder.HasIndex(x => x.SourceWarehouse).HasDatabaseName("IX_SitRoute_SourceWarehouse");
            builder.HasIndex(x => x.TargetWarehouse).HasDatabaseName("IX_SitRoute_TargetWarehouse");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_SitRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}