using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WoLineSerialConfiguration : BaseEntityConfiguration<WoLineSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WoLineSerial> builder)
        {
            builder.ToTable("RII_WO_LINE_SERIAL");

            builder.Property(x => x.LineId).IsRequired();

            builder.HasIndex(x => x.LineId).HasDatabaseName("IX_WoLineSerial_LineId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WoLineSerial_IsDeleted");

            builder.HasOne(x => x.Line)
                .WithMany(x => x.SerialLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}