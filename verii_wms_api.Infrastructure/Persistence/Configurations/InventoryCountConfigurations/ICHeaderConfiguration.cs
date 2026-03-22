using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class ICHeaderConfiguration : BaseHeaderEntityConfiguration<IcHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<IcHeader> builder)
        {
            builder.ToTable("RII_IC_HEADER");

            builder.Property(x => x.BranchCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.ProjectCode).HasMaxLength(20);
            builder.Property(x => x.DocumentType).HasMaxLength(10).IsRequired();
            builder.Property(x => x.CellCode).HasMaxLength(35);
            builder.Property(x => x.WarehouseCode).HasMaxLength(20);
            builder.Property(x => x.ProductCode).HasMaxLength(50);
            builder.Property(x => x.YearCode).HasMaxLength(4).IsRequired();
            builder.Property(x => x.Description1).HasMaxLength(50);
            builder.Property(x => x.Description2).HasMaxLength(100);
            builder.Property(x => x.PriorityLevel);
            builder.Property(x => x.Type).IsRequired();

            builder.HasIndex(x => x.BranchCode).HasDatabaseName("IX_ICHeader_BranchCode");
            builder.HasIndex(x => x.PlannedDate).HasDatabaseName("IX_ICHeader_PlannedDate");
            builder.HasIndex(x => x.WarehouseCode).HasDatabaseName("IX_ICHeader_WarehouseCode");
            builder.HasIndex(x => x.ProductCode).HasDatabaseName("IX_ICHeader_ProductCode");
            builder.HasIndex(x => x.YearCode).HasDatabaseName("IX_ICHeader_YearCode");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ICHeader_IsDeleted");

            builder.HasMany(x => x.ImportLines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.TerminalLines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}