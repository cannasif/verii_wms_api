using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SrtImportLineConfiguration : BaseImportLineEntityConfiguration<SrtImportLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SrtImportLine> builder)
        {
            builder.ToTable("RII_SRT_IMPORT_LINE");

            builder.Property(x => x.HeaderId).IsRequired();
            builder.Property(x => x.LineId);
            

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_SrtImportLine_HeaderId");
            builder.HasIndex(x => x.LineId).HasDatabaseName("IX_SrtImportLine_LineId");
            builder.HasIndex(x => x.StockCode).HasDatabaseName("IX_SrtImportLine_StockCode");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_SrtImportLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Line)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}