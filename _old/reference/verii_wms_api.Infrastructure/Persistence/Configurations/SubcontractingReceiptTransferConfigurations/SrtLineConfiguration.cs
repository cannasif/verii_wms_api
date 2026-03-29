using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SrtLineConfiguration : BaseLineEntityConfiguration<SrtLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SrtLine> builder)
        {
            builder.ToTable("RII_SRT_LINE");

            builder.Property(x => x.HeaderId).IsRequired();
            

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_SrtLine_HeaderId");
            builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_SrtLine_StockCode");
            builder.HasIndex(x => x.ErpOrderNo).HasDatabaseName("IX_SrtLine_ErpOrderNo");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_SrtLine_IsDeleted");

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