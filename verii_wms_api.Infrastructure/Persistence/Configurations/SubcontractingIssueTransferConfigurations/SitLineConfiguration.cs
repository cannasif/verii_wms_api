using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SitLineConfiguration : BaseLineEntityConfiguration<SitLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SitLine> builder)
        {
            builder.ToTable("RII_SIT_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            

            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_SitLine_HeaderId");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_SitLine_StockCode");

            builder.HasIndex(x => x.ErpOrderNo)
                .HasDatabaseName("IX_SitLine_ErpOrderNo");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_SitLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            

            builder.HasMany(x => x.ImportLines)
                .WithOne(x => x.Line)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}