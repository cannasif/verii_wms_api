using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SitTerminalLineConfiguration : BaseEntityConfiguration<SitTerminalLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SitTerminalLine> builder)
        {
            builder.ToTable("RII_SIT_TERMINAL_LINE");

            builder.Property(x => x.HeaderId)
                .IsRequired();

            builder.Property(x => x.TerminalUserId)
                .IsRequired();

            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_SitTerminalLine_HeaderId");

            builder.HasIndex(x => x.TerminalUserId)
                .HasDatabaseName("IX_SitTerminalLine_TerminalUserId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_SitTerminalLine_IsDeleted");

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