using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrRouteConfiguration : BaseRouteEntityConfiguration<GrRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrRoute> builder)
        {
            builder.ToTable("RII_GR_ROUTE");

            builder.Property(x => x.ImportLineId)
                .IsRequired()
                .HasColumnName("ImportLineId");

            builder.Property(x => x.SourceWarehouse)
                .HasColumnName("SourceWarehouse");

            builder.Property(x => x.TargetWarehouse)
                .HasColumnName("TargetWarehouse");


            builder.HasIndex(x => x.ImportLineId)
                .HasDatabaseName("IX_GrRoute_ImportLineId");

            builder.HasIndex(x => x.SerialNo)
                .HasDatabaseName("IX_GrRoute_SerialNo");

            builder.HasIndex(x => x.SourceWarehouse)
                .HasDatabaseName("IX_GrRoute_SourceWarehouse");

            builder.HasIndex(x => x.TargetWarehouse)
                .HasDatabaseName("IX_GrRoute_TargetWarehouse");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_GrRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany(x => x.Routes)
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}