using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PtRouteConfiguration : BaseRouteEntityConfiguration<PtRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PtRoute> builder)
        {
            builder.ToTable("RII_PT_ROUTE");
            builder.Property(x => x.ImportLineId)
                .IsRequired();

            

            builder.HasIndex(x => x.ImportLineId)
                .HasDatabaseName("IX_PtRoute_ImportLineId");

            builder.HasIndex(x => x.SerialNo)
                .HasDatabaseName("IX_PtRoute_SerialNo");

            builder.HasIndex(x => x.SourceWarehouse)
                .HasDatabaseName("IX_PtRoute_SourceWarehouse");

            builder.HasIndex(x => x.TargetWarehouse)
                .HasDatabaseName("IX_PtRoute_TargetWarehouse");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PtRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}