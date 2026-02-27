using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SidebarmenuLineConfiguration : BaseEntityConfiguration<SidebarmenuLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SidebarmenuLine> builder)
        {
            // Table name
            builder.ToTable("RII_PLATFORM_SIDEBARMENU_LINE");
            
            // Properties configuration

            builder.Property(x => x.HeaderId)
                .IsRequired()
                .HasColumnName("HeaderId");

            builder.Property(x => x.Page)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Page");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Title");

            builder.Property(x => x.Description)
                .HasMaxLength(200)
                .HasColumnName("Description");

            builder.Property(x => x.Icon)
                .HasMaxLength(50)
                .HasColumnName("Icon");

            // Base entity handled by BaseEntityConfiguration

            // Indexes
            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_SidebarmenuLine_HeaderId");

            builder.HasIndex(x => x.Page)
                .IsUnique()
                .HasDatabaseName("IX_SidebarmenuLine_Page");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_SidebarmenuLine_IsDeleted");

            // Relationships
            builder.HasOne(x => x.Header)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);

                            builder.HasData(
                new SidebarmenuLine { Id = 1, HeaderId = 1, Page = "tamamlananMalKabulListesi", Title = "sidebar.malKabul.completedList", Description = "sidebar.malKabul.completedListDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 2, HeaderId = 2, Page = "sevkiyatOlustur", Title = "sidebar.sevkiyat.create", Description = "sidebar.sevkiyat.createDesc", Icon = "HiOutlineTruck", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 3, HeaderId = 2, Page = "tamamlananSevkiyatListesi", Title = "sidebar.sevkiyat.completedList", Description = "sidebar.sevkiyat.completedListDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }, 
                new SidebarmenuLine { Id = 4, HeaderId = 3, Page = "depolarArasiTransferOlustur", Title = "sidebar.transfer.createInterWarehouse", Description = "sidebar.transfer.createInterWarehouseDesc", Icon = "HiOutlineRefresh", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 5, HeaderId = 3, Page = "sipariseIstinadenDepolarArasiTransfer", Title = "sidebar.transfer.orderBased", Description = "sidebar.transfer.orderBasedDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 6, HeaderId = 3, Page = "tamamlanmisTransferEmirleri", Title = "sidebar.transfer.completed", Description = "sidebar.transfer.completedDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 7, HeaderId = 3, Page = "uretimeTransfer", Title = "sidebar.transfer.productionBased", Description = "sidebar.transfer.productionBasedDesc", Icon = "HiOutlineCollection", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 8, HeaderId = 3, Page = "ambarCikisOlustur", Title = "sidebar.transfer.warehouseExit", Description = "sidebar.transfer.warehouseExitDesc", Icon = "HiOutlineArrowLeft", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 9, HeaderId = 4, Page = "sayimEmriOlustur", Title = "sidebar.sayim.createOrder", Description = "sidebar.sayim.createOrderDesc", Icon = "HiOutlineCalculator", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 10, HeaderId = 4, Page = "tamamlananSayimListesi", Title = "sidebar.sayim.completedList", Description = "sidebar.sayim.completedListDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 11, HeaderId = 5, Page = "hucreEmriOlustur", Title = "sidebar.hucreTakibi.createOrder", Description = "sidebar.hucreTakibi.createOrderDesc", Icon = "HiOutlineViewGrid", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 12, HeaderId = 6, Page = "tamamlananUretimListesi", Title = "sidebar.uretim.completedList", Description = "sidebar.uretim.completedListDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 13, HeaderId = 7, Page = "tamamlananPaketlemeListesi", Title = "sidebar.paketleme.completedList", Description = "sidebar.paketleme.completedListDesc", Icon = "HiOutlineDocumentText", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 14, HeaderId = 8, Page = "kullaniciIslemleri", Title = "sidebar.kullanici.islemleri", Description = "sidebar.kullanici.islemleriDesc", Icon = "HiOutlineUsers", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 15, HeaderId = 8, Page = "platformGrupIslemleri", Title = "sidebar.platformGrup.islemleri", Description = "sidebar.platformGrup.islemleriDesc", Icon = "HiOutlineUserGroup", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 16, HeaderId = 8, Page = "platformKullaniciGrupEslemeIslemleri", Title = "sidebar.platformKullaniciGrup.islemleri", Description = "sidebar.platformKullaniciGrup.islemleriDesc", Icon = "HiOutlineUserGroup", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 17, HeaderId = 8, Page = "mobilGrupIslemleri", Title = "sidebar.mobilGrup.islemleri", Description = "sidebar.mobilGrup.islemleriDesc", Icon = "HiOutlineUserGroup", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SidebarmenuLine { Id = 18, HeaderId = 8, Page = "mobilKullaniciGrupEslemeIslemleri", Title = "sidebar.mobilKullaniciGrup.islemleri", Description = "sidebar.mobilKullaniciGrup.islemleriDesc", Icon = "HiOutlineUserGroup", CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}