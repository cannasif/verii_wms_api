using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WtImportLineConfiguration : BaseImportLineEntityConfiguration<WtImportLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WtImportLine> builder)
        {
            builder.ToTable("RII_WT_IMPORT_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            builder.Property(x => x.LineId);

            

            // Indexes
            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_TrImportLine_HeaderId");

            builder.HasIndex(x => x.LineId)
                .HasDatabaseName("IX_TrImportLine_LineId");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_TrImportLine_StockCode");

            

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_TrImportLine_IsDeleted");

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