using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class IcTerminalLineConfiguration : BaseEntityConfiguration<IcTerminalLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<IcTerminalLine> builder)
        {
            builder.ToTable("RII_IC_TERMINAL_LINE");

            builder.Property(x => x.HeaderId).IsRequired();
            builder.Property(x => x.TerminalUserId).IsRequired();

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_IcTerminalLine_HeaderId");
            builder.HasIndex(x => x.TerminalUserId).HasDatabaseName("IX_IcTerminalLine_TerminalUserId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_IcTerminalLine_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.TerminalLines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.TerminalUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}