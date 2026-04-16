# ServiceAllocation UAT Guide

Bu rehber operasyon ve test ekipleri için hazırlanmıştır.

Amaç:
- hangi sırayla işlem yapılacağını göstermek
- hangi ekranda ne beklenmesi gerektiğini netleştirmek
- hata halinde ilk bakılacak yerleri belirtmek

## 1. UAT Ön Koşulları

Başlamadan önce kontrol edin:
- migration uygulanmış olmalı
- backend ayakta olmalı
- web uygulaması ayakta olmalı
- test kullanıcısının ilgili ekranlara erişim yetkisi olmalı
- test edilecek müşteri, stok ve depo id bilgileri hazır olmalı

## 2. Test Sırası

Önerilen sıra:

1. Servis vakası oluştur
2. İlk servis kalemlerini ekle
3. Vaka detay ekranını kontrol et
4. Allocation satırlarını kontrol et
5. Manuel recompute çalıştır
6. Gerçek operasyon belgelerini sırayla işlet
7. Timeline ve link kayıtlarını tekrar kontrol et

## 3. Web Ekranlarında Tıklama Sırası

### Adım 1 - Servis vaka listesine git

Ekran:
- `/service-allocation/cases`

Beklenen:
- servis vaka listesi açılmalı
- sağ üstte `Create Service Case` butonu görünmeli

### Adım 2 - Yeni servis vakası aç

Tıklama:
- `Create Service Case`

Ekran:
- `/service-allocation/cases/new`

Doldurulacak temel alanlar:
- `Case No`
- `Customer Code`
- `Incoming Stock Code`
- `Incoming Stock Id`
- `Serial No`
- `Status`
- `Received At`

Beklenen:
- form kaydedilebilmeli
- başarılı kayıt sonrası detay/timeline ekranına yönlenmeli

### Adım 3 - İlk servis kalemini ekle

Aynı form ekranında veya edit ekranında:
- `Initial / Additional Service Line` alanlarını doldur

Yedek parça için örnek:
- `Line Type = Spare Part`
- `Process Type = Service Repair`
- `Stock Code`
- `Stock Id`
- `Quantity`
- `ERP Order No`
- `ERP Order Id`

Beklenen:
- kayıt sonrası servis kalemi oluşmalı
- uygun ise allocation satırı oluşmalı

### Adım 4 - Vaka detayını kontrol et

Ekran:
- `/service-allocation/cases/{id}`

Beklenen:
- vaka bilgileri görünmeli
- `Case Lines` tablosunda girilen kalemler görünmeli
- `Warehouse Timeline` bölümü ilk durumda boş ya da sınırlı olabilir

### Adım 5 - Manual recompute çalıştır

Tıklama:
- `Recompute Allocation`

Beklenen:
- başarılı toast mesajı gelmeli
- timeline sayfası açık kalmalı
- allocation backend tarafında yeniden hesaplanmalı

### Adım 6 - Edit ekranını kontrol et

Tıklama:
- listede veya detayda `Edit`

Ekran:
- `/service-allocation/cases/{id}/edit`

Beklenen:
- kayıtlı bilgiler formda dolu gelmeli
- alttaki `Existing Service Lines` tablosunda önceki satırlar görünmeli

## 4. Operasyon Sonrası Beklentiler

Gerçek işlem akışında `WI`, `WT`, `WO`, `SH`, `SIT`, `SRT` belgeleri tamamlandıktan sonra aşağıdakiler kontrol edilir.

### WI sonrası

Beklenen:
- timeline içinde `WI`
- `LinkPurpose = Intake`

### WT sonrası

Beklenen:
- timeline içinde `WT`
- `LinkPurpose = InternalTransfer`

### WO sonrası

Beklenen:
- timeline içinde `WO`
- `LinkPurpose = SparePartSupply`

### SH sonrası

Beklenen:
- timeline içinde `SH`
- `LinkPurpose = Shipment`

### SIT sonrası

Beklenen:
- timeline içinde `SIT`
- `LinkPurpose = RepairOperation`

### SRT sonrası

Beklenen:
- timeline içinde `SRT`
- `LinkPurpose = ReturnToMainWarehouse`

## 5. Hangi Ekranda Ne Beklenmeli

### Servis vaka listesi

Beklenen:
- kayıtlı vakalar listelenmeli
- `Case No`, `Customer`, `Incoming Stock`, `Serial`, `Status` görünmeli

### Servis vaka formu

Beklenen:
- create ve edit aynı ekran mantığında çalışmalı
- edit modunda mevcut bilgiler dolu gelmeli

### Timeline ekranı

Beklenen:
- üst kartta vaka özeti görünmeli
- `Case Lines` bölümünde servis kalemleri listelenmeli
- `Warehouse Timeline` bölümünde belge hareket zinciri görünmeli
- `Linked Documents` sayısı mantıklı artmalı

## 6. Hata Olursa İlk Nerelere Bakılmalı

### Durum 1 - Servis kalemi oluşuyor ama allocation oluşmuyor

İlk kontrol:
- `ServiceCaseLine` içinde `LineType` gerçekten `SparePart` veya `ReplacementProduct` mı
- `StockId` dolu mu
- `ErpOrderNo` ve `ErpOrderId` dolu mu

Kod tarafı:
- [ServiceCaseLineService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/ServiceAllocation/Application/Core/Services/ServiceCaseLineService.cs:1)

### Durum 2 - Operasyon tamamlanıyor ama timeline’a link düşmüyor

İlk kontrol:
- ilgili operasyon `commit` sonrasında orkestrasyon çağırıyor mu
- allocation satırlarında `SourceModule` ve `SourceHeaderId` var mı
- servis kaleminde doğru `ErpOrderNo` / `ErpOrderId` var mı

Kod tarafı:
- [OperationAllocationOrchestrator.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/ServiceAllocation/Application/Core/Services/OperationAllocationOrchestrator.cs:1)
- [WiHeaderService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/WarehouseInbound/Application/Core/Services/WiHeaderService.cs:656)
- [WtHeaderService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/WarehouseTransfer/Application/Core/Services/WtHeaderService.cs:753)
- [WoHeaderService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/WarehouseOutbound/Application/Core/Services/WoHeaderService.cs:718)
- [ShHeaderService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/Shipping/Application/Core/Services/ShHeaderService.cs:1)
- [SitHeaderService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/SubcontractingIssueTransfer/Application/Core/Services/SitHeaderService.cs:1)
- [SrtHeaderService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/SubcontractingReceiptTransfer/Application/Core/Services/SrtHeaderService.cs:1)

### Durum 3 - Recompute çalışıyor ama dağıtım beklenen gibi değil

İlk kontrol:
- aynı `StockId` için birden fazla allocation satırı var mı
- `PriorityNo` değerleri doğru mu
- `RequestedQuantity`, `FulfilledQuantity` doğru mu
- ERP bakiye değeri beklendiği gibi mi geliyor

Kod tarafı:
- [AllocationEngine.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/ServiceAllocation/Application/Core/Services/AllocationEngine.cs:1)

### Durum 4 - Form kaydı başarısız

İlk kontrol:
- `CaseNo` duplicate mi
- `CustomerCode` boş mu
- yedek parça satırında `StockId` boş mu
- `Quantity <= 0` mı

Kod tarafı:
- [ServiceCaseService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/ServiceAllocation/Application/Core/Services/ServiceCaseService.cs:1)
- [ServiceCaseLineService.cs](/Users/cannasif/Documents/V3rii/verii_wms_api/srcWms/Wms/Modules/ServiceAllocation/Application/Core/Services/ServiceCaseLineService.cs:1)

## 7. UAT Başarı Kriteri

Test başarılı sayılır if:
- servis vakası oluşturulabiliyor
- servis kalemleri kaydoluyor
- uygun kalemlerde allocation otomatik oluşuyor
- operasyon sonrası belge linkleri otomatik geliyor
- timeline zinciri kullanıcıya anlamlı görünür hale geliyor
- manuel recompute beklenen sonucu veriyor

## 8. UAT Sonu Kararı

Eğer aşağıdakiler sağlanıyorsa yapı kabul edilebilir:
- create/edit akışı stabil
- timeline doğru akıyor
- operasyon sonrası linkleme çalışıyor
- hakediş yeniden hesaplama çalışıyor
- kritik validasyonlar hatalı datayı engelliyor
