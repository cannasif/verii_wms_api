using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WtTerminalLineConfiguration : BaseEntityConfiguration<WtTerminalLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WtTerminalLine> builder)
        {
            builder.ToTable("RII_WT_TerminalLine");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            builder.Property(x => x.TerminalUserId)
                .IsRequired();

            // Quantity alanı WtTerminalLine modelinde bulunmuyor

            // Indexes
            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_WtTerminalLine_HeaderId");

            builder.HasIndex(x => x.TerminalUserId)
                .HasDatabaseName("IX_WtTerminalLine_TerminalUserId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_WtTerminalLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany()
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.TerminalUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}