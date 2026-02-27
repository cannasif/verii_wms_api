using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class MobilemenuLineConfiguration : BaseEntityConfiguration<MobilemenuLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MobilemenuLine> builder)
        {
            // Table name
            builder.ToTable("RII_MOBILMENU_LINE");
            
            // Properties configuration

            builder.Property(x => x.ItemId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("ItemId");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Title");

            builder.Property(x => x.Icon)
                .HasMaxLength(100)
                .HasColumnName("Icon");

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .HasColumnName("Description");

            builder.Property(x => x.HeaderId)
                .IsRequired()
                .HasColumnName("HeaderId");

            // Base entity handled by BaseEntityConfiguration

            // Relationships
            builder.HasOne(x => x.Header)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // User relationships handled by BaseEntityConfiguration

                builder.HasData(
                //MAL KABUL
                new MobilemenuLine { Id = 1, ItemId = "iade-girisi", Title = "iadeGirisi", Icon = "return-up-back-outline", Description = "iadeGirisiDesc", HeaderId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 2, ItemId = "irsaliye-fatura", Title = "irsaliyeFatura", Icon = "document-text-outline", Description = "irsaliyeFaturaDesc", HeaderId = 1, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //SEVKİYAT
                new MobilemenuLine { Id = 3, ItemId = "sevkiyat-emri", Title = "sevkiyatEmri", Icon = "send-outline", Description = "sevkiyatEmriDesc", HeaderId = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 4, ItemId = "sevkiyat-kontrol", Title = "sevkiyatKontrol", Icon = "send-outline", Description = "sevkiyatKontrolDesc", HeaderId = 2, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //TRANSFER
                new MobilemenuLine { Id = 5, ItemId = "depo-transferi", Title = "depoTransferi", Icon = "archive-outline", Description = "depoTransferiDesc", HeaderId = 3, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 6, ItemId = "ambar-giris", Title = "ambarGiris", Icon = "enter-outline", Description = "ambarGirisDesc", HeaderId = 3, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 7, ItemId = "ambar-cikis", Title = "ambarCikis", Icon = "exit-outline", Description = "ambarCikisDesc", HeaderId = 3, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 8, ItemId = "planli-depo-transferi", Title = "planliDepoTransferi", Icon = "calendar-outline", Description = "planliDepoTransferiDesc", HeaderId = 3, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 9, ItemId = "planli-ambar-cikis", Title = "planliAmbarCikis", Icon = "calendar-clear-outline", Description = "planliAmbarCikisDesc", HeaderId = 3, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //SAYIM
                new MobilemenuLine { Id = 10, ItemId = "sayim-girisi", Title = "sayimGirisi", Icon = "list-outline", Description = "sayimGirisiDesc", HeaderId = 4, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //HUCRE TAKİBI
                new MobilemenuLine { Id = 11, ItemId = "hucre-transferi", Title = "hucreBilgisi", Icon = "move-outline", Description = "hucreBilgisiDesc", HeaderId = 5, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 12, ItemId = "hucreler-arasi-transfer", Title = "hucrelerArasiTransfer", Icon = "swap-vertical-outline", Description = "hucrelerArasiTransferDesc", HeaderId = 5, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 13, ItemId = "planli-hucre-transferi", Title = "planliHucreTransferi", Icon = "calendar-outline", Description = "planliHucreTransferiDesc", HeaderId = 5, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //URETIM
                new MobilemenuLine { Id = 14, ItemId = "uretim-sonu-kaydi", Title = "uretimSonuKaydi", Icon = "checkmark-done-outline", Description = "uretimSonuKaydiDesc", HeaderId = 6, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 15, ItemId = "kiosk", Title = "kiosk", Icon = "hammer-outline", Description = "kioskDesc", HeaderId = 6, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //PAKETLEME
                new MobilemenuLine { Id = 16, ItemId = "paketleme-girisi", Title = "paketlemeGirisi", Icon = "gift-outline", Description = "paketlemeGirisiDesc", HeaderId = 7, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new MobilemenuLine { Id = 17, ItemId = "paketleme-islemleri", Title = "paketlemeIslemleri", Icon = "layers-outline", Description = "paketlemeIslemleriDesc", HeaderId = 7, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //SESLI KOMUT
                new MobilemenuLine { Id = 18, ItemId = "sesli-komut-test", Title = "sesliKomutTest", Icon = "repeat-outline", Description = "sesliKomutTestDesc", HeaderId = 8, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                //GENEL BILGI
                new MobilemenuLine { Id = 19, ItemId = "stok-detay-ekrani", Title = "stokDetayEkrani", Icon = "analytics-outline", Description = "stokDetayEkraniDesc", HeaderId = 9, CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
    
}