# WMS Web API

Warehouse Management System (Depo Yönetim Sistemi) Web API projesi.

## Özellikler

- ✅ .NET 8.0 Web API
- ✅ Entity Framework Core
- ✅ SQL Server veritabanı desteği
- ✅ Swagger/OpenAPI dokümantasyonu
- ✅ CORS desteği
- ✅ Soft Delete implementasyonu

## Varlıklar (Entities)

### Warehouse (Depo)
- Depo bilgileri (ad, açıklama, adres)
- Şehir, ülke, posta kodu

### Product (Ürün)
- SKU, ad, açıklama
- Fiyat, kategori, marka
- Birim, ağırlık, boyutlar
- Depo ilişkisi

### Stock (Stok)
- Miktar, rezerve miktar
- Minimum/maksimum stok seviyeleri
- Lokasyon bilgisi
- Ürün ve depo ilişkileri

### StockMovement (Stok Hareketi)
- Hareket türü (Giriş, Çıkış, Transfer, Düzeltme)
- Miktar, sebep, referans
- Hareket tarihi

## API Endpoints

### Warehouses
- `GET /api/warehouses` - Tüm depoları listele
- `GET /api/warehouses/{id}` - Depo detayı
- `POST /api/warehouses` - Yeni depo oluştur
- `PUT /api/warehouses/{id}` - Depo güncelle
- `DELETE /api/warehouses/{id}` - Depo sil (soft delete)

### Products
- `GET /api/products` - Tüm ürünleri listele
- `GET /api/products/{id}` - Ürün detayı
- `GET /api/products/sku/{sku}` - SKU ile ürün ara
- `GET /api/products/warehouse/{warehouseId}` - Depoya göre ürünler
- `POST /api/products` - Yeni ürün oluştur
- `PUT /api/products/{id}` - Ürün güncelle
- `DELETE /api/products/{id}` - Ürün sil (soft delete)

### Stocks
- `GET /api/stocks` - Tüm stokları listele
- `GET /api/stocks/{id}` - Stok detayı
- `GET /api/stocks/product/{productId}` - Ürüne göre stoklar
- `GET /api/stocks/warehouse/{warehouseId}` - Depoya göre stoklar
- `GET /api/stocks/low-stock` - Düşük stok uyarıları
- `POST /api/stocks/adjust/{id}` - Stok düzeltme
- `POST /api/stocks` - Yeni stok oluştur
- `PUT /api/stocks/{id}` - Stok güncelle
- `DELETE /api/stocks/{id}` - Stok sil (soft delete)

### Stock Movements
- `GET /api/stockmovements` - Tüm hareketleri listele
- `GET /api/stockmovements/{id}` - Hareket detayı
- `GET /api/stockmovements/product/{productId}` - Ürüne göre hareketler
- `GET /api/stockmovements/stock/{stockId}` - Stoka göre hareketler
- `GET /api/stockmovements/type/{movementType}` - Türe göre hareketler
- `GET /api/stockmovements/date-range` - Tarih aralığına göre hareketler
- `POST /api/stockmovements/stock-in` - Stok giriş
- `POST /api/stockmovements/stock-out` - Stok çıkış
- `POST /api/stockmovements` - Yeni hareket oluştur

## Kurulum

1. Projeyi klonlayın
2. SQL Server bağlantı dizesini `appsettings.json` dosyasında güncelleyin
3. Migration'ları çalıştırın:
   ```bash
   dotnet ef database update
   ```
4. Projeyi çalıştırın:
   ```bash
   dotnet run
   ```

## Swagger UI

Proje çalıştırıldığında Swagger UI otomatik olarak açılır:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7000

## Veritabanı

Proje SQL Server LocalDB kullanmaktadır. Bağlantı dizesi:
```
Server=(localdb)\\mssqllocaldb;Database=WmsDatabase;Trusted_Connection=true;MultipleActiveResultSets=true
```

## Seed Data

Proje başlangıçta 2 adet örnek depo verisi ile gelir:
- Ana Depo (İstanbul)
- Yedek Depo (Ankara)