using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SidebarmenuHeaderConfiguration : BaseEntityConfiguration<SidebarmenuHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SidebarmenuHeader> builder)
        {
            // Table name
            builder.ToTable("RII_PLATFORM_SIDEBARMENU_HEADER");

            // Properties configuration

            builder.Property(x => x.MenuKey)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("MenuKey");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Title");

            builder.Property(x => x.Icon)
                .HasMaxLength(50)
                .HasColumnName("Icon");

            builder.Property(x => x.Color)
                .HasMaxLength(20)
                .HasColumnName("Color");

            builder.Property(x => x.DarkColor)
                .HasMaxLength(20)
                .HasColumnName("DarkColor");

            builder.Property(x => x.ShadowColor)
                .HasMaxLength(20)
                .HasColumnName("ShadowColor");

            builder.Property(x => x.DarkShadowColor)
                .HasMaxLength(20)
                .HasColumnName("DarkShadowColor");

            builder.Property(x => x.TextColor)
                .HasMaxLength(20)
                .HasColumnName("TextColor");

            builder.Property(x => x.DarkTextColor)
                .HasMaxLength(20)
                .HasColumnName("DarkTextColor");

            builder.Property(x => x.RoleLevel)
                .IsRequired()
                .HasColumnName("RoleLevel");

            // Base entity handled by BaseEntityConfiguration

            // Indexes
            builder.HasIndex(x => x.MenuKey)
                .IsUnique()
                .HasDatabaseName("IX_SidebarmenuHeader_MenuKey");

            builder.HasIndex(x => x.RoleLevel)
                .HasDatabaseName("IX_SidebarmenuHeader_RoleLevel");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_SidebarmenuHeader_IsDeleted");

            // Relationships
            builder.HasMany(x => x.Lines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasData(
                new SidebarmenuHeader { Id = 1, MenuKey = "malKabul", Title = "sidebar.malKabul.title", Icon = "HiOutlineCube", Color = "blue-100", DarkColor = "blue-700", ShadowColor = "blue-300", DarkShadowColor = "blue-500", TextColor = "blue-600", DarkTextColor = "blue-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 2, MenuKey = "sevkiyat", Title = "sidebar.sevkiyat.title", Icon = "HiOutlineTruck", Color = "purple-100", DarkColor = "purple-700", ShadowColor = "purple-300", DarkShadowColor = "purple-500", TextColor = "purple-600", DarkTextColor = "purple-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 3, MenuKey = "transfer", Title = "sidebar.transfer.title", Icon = "HiOutlineRefresh", Color = "cyan-100", DarkColor = "cyan-700", ShadowColor = "cyan-300", DarkShadowColor = "cyan-500", TextColor = "cyan-600", DarkTextColor = "cyan-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 4, MenuKey = "sayim", Title = "sidebar.sayim.title", Icon = "HiOutlineCalculator", Color = "orange-100", DarkColor = "orange-700", ShadowColor = "orange-300", DarkShadowColor = "orange-500", TextColor = "orange-600", DarkTextColor = "orange-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 5, MenuKey = "hucreTakibi", Title = "sidebar.hucreTakibi.title", Icon = "HiOutlineViewGrid", Color = "indigo-100", DarkColor = "indigo-700", ShadowColor = "indigo-300", DarkShadowColor = "indigo-500", TextColor = "indigo-600", DarkTextColor = "indigo-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 6, MenuKey = "uretim", Title = "sidebar.uretim.title", Icon = "HiOutlineCollection", Color = "yellow-100", DarkColor = "yellow-700", ShadowColor = "yellow-300", DarkShadowColor = "yellow-500", TextColor = "yellow-600", DarkTextColor = "yellow-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 7, MenuKey = "paketleme", Title = "sidebar.paketleme.title", Icon = "HiOutlineCollection", Color = "red-100", DarkColor = "red-700", ShadowColor = "red-300", DarkShadowColor = "red-500", TextColor = "red-600", DarkTextColor = "red-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 8, MenuKey = "yonetim", Title = "sidebar.yonetim.title", Icon = "HiOutlineCog", Color = "green-100", DarkColor = "green-700", ShadowColor = "green-300", DarkShadowColor = "green-500", TextColor = "green-600", DarkTextColor = "green-400", RoleLevel=3, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 9, MenuKey = "parametreler", Title = "sidebar.parametreler.title", Icon = "RiSettingsFill", Color = "teal-100", DarkColor = "teal-700", ShadowColor = "teal-300", DarkShadowColor = "teal-500", TextColor = "teal-600", DarkTextColor = "teal-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuHeader { Id = 10, MenuKey = "raporlar", Title = "sidebar.raporlar.title", Icon = "LuFileChartLine", Color = "fuchsia-100", DarkColor = "fuchsia-700", ShadowColor = "fuchsia-300", DarkShadowColor = "fuchsia-500", TextColor = "fuchsia-600", DarkTextColor = "fuchsia-400", RoleLevel = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

        }
    }
}