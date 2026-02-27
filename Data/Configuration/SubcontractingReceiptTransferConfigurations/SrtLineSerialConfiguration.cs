using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SrtLineSerialConfiguration : BaseLineSerialEntityConfiguration<SrtLineSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SrtLineSerial> builder)
        {
            builder.ToTable("RII_SRT_LINE_SERIAL");

            builder.Property(x => x.LineId).IsRequired();

            builder.HasIndex(x => x.LineId).HasDatabaseName("IX_SrtLineSerial_LineId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_SrtLineSerial_IsDeleted");

            builder.HasOne(x => x.Line)
                .WithMany(x => x.SerialLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}