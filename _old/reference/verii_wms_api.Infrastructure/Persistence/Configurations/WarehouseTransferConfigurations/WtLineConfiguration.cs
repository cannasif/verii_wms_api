using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WtLineConfiguration : BaseLineEntityConfiguration<WtLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WtLine> builder)
        {
            builder.ToTable("RII_WT_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            

            // Indexes
            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_WtLine_HeaderId");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_WtLine_StockCode");

            builder.HasIndex(x => x.ErpOrderNo)
                .HasDatabaseName("IX_WtLine_ErpOrderNo");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_WtLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany()
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);


            // Navigation properties

            builder.HasMany(x => x.ImportLines)
                .WithOne(il => il.Line)
                .HasForeignKey(il => il.LineId)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}