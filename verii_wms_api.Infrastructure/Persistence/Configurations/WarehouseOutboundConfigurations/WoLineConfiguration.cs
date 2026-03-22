using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WoLineConfiguration : BaseLineEntityConfiguration<WoLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WoLine> builder)
        {
            builder.ToTable("RII_WO_LINE");

            builder.Property(x => x.HeaderId).IsRequired();
            

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_WoLine_HeaderId");
            builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_WoLine_StockCode");
            builder.HasIndex(x => x.ErpOrderNo).HasDatabaseName("IX_WoLine_ErpOrderNo");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WoLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            

            builder.HasMany(x => x.ImportLines)
                .WithOne(x => x.Line)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Terminal lines are tracked at header level; no explicit Line relationship configured
        }
    }
}