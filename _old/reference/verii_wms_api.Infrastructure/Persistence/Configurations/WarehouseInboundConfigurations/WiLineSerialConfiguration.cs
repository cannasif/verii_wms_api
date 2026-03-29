using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WiLineSerialConfiguration : BaseLineSerialEntityConfiguration<WiLineSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WiLineSerial> builder)
        {
            builder.ToTable("RII_WI_LINE_SERIAL");

            builder.Property(x => x.LineId).IsRequired();

            builder.HasIndex(x => x.LineId).HasDatabaseName("IX_WiLineSerial_LineId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WiLineSerial_IsDeleted");

            builder.HasOne(x => x.Line)
                .WithMany(x => x.SerialLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}