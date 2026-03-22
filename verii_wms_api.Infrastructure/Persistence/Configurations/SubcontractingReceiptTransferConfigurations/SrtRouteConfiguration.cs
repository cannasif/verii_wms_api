using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SrtRouteConfiguration : BaseRouteEntityConfiguration<SrtRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SrtRoute> builder)
        {
            builder.ToTable("RII_SRT_ROUTE");

            builder.Property(x => x.ImportLineId).IsRequired();
            

            builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_SrtRoute_ImportLineId");
            builder.HasIndex(x => x.SerialNo).HasDatabaseName("IX_SrtRoute_SerialNo");
            builder.HasIndex(x => x.SourceWarehouse).HasDatabaseName("IX_SrtRoute_SourceWarehouse");
            builder.HasIndex(x => x.TargetWarehouse).HasDatabaseName("IX_SrtRoute_TargetWarehouse");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_SrtRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}