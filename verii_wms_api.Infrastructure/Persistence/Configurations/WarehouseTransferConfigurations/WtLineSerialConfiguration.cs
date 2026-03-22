using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WtLineSerialConfiguration : BaseEntityConfiguration<WtLineSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WtLineSerial> builder)
        {
            builder.ToTable("RII_WT_LINE_SERIAL");

            builder.Property(x => x.LineId).IsRequired();

            builder.HasIndex(x => x.LineId).HasDatabaseName("IX_WtLineSerial_LineId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_WtLineSerial_IsDeleted");

            builder.HasOne(x => x.Line)
                .WithMany(x => x.SerialLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}