# ServiceAllocation Test Scenarios

Bu doküman `ServiceAllocation` modülü için uçtan uca test senaryolarını ve örnek API payload'larını içerir.

## 1. Servis Vakası Açma

Amaç:
- Arızalı ürün sisteme servis vakası olarak girilsin.
- Giriş ürünü, müşteri ve ilk durum kaydedilsin.

Endpoint:
- `POST /api/ServiceCase`

Örnek payload:

```json
{
  "caseNo": "SRV-20260414-001",
  "customerCode": "CARI001",
  "customerId": 101,
  "incomingStockCode": "CM-001",
  "incomingStockId": 5001,
  "incomingSerialNo": "SN-0001",
  "intakeWarehouseId": 1,
  "currentWarehouseId": 1,
  "diagnosisNote": "Camasir makinesi arizali geldi, ilk kontrol bekleniyor.",
  "status": 2,
  "receivedAt": "2026-04-14",
  "branchCode": "0"
}
```

Beklenen:
- `ServiceCase` kaydı oluşur.
- Timeline ekranında vaka görüntülenebilir.

## 2. Servis Kalemi Ekleme - Yedek Parça

Amaç:
- Servis vakasına değişecek parça eklenir.
- Eğer `ErpOrderNo` ve `ErpOrderId` doluysa ilgili `OrderAllocationLine` otomatik oluşur/güncellenir.

Endpoint:
- `POST /api/ServiceCaseLine`

Örnek payload:

```json
{
  "serviceCaseId": 1,
  "lineType": 1,
  "processType": 1,
  "stockCode": "PRC-004",
  "stockId": 7004,
  "quantity": 2,
  "unit": "ADET",
  "erpOrderNo": "SIP-2026-145",
  "erpOrderId": "987654",
  "description": "Motor kayis seti degisimi",
  "branchCode": "0"
}
```

Beklenen:
- `ServiceCaseLine` oluşur.
- Aynı `StockId + ErpOrderId + ProcessType` için allocation satırı oluşur ya da `RequestedQuantity` güncellenir.

## 3. Servis Kalemi Ekleme - İşçilik

Amaç:
- İşçilik satırı stoktan bağımsız takip edilsin.
- Allocation oluşmamalı.

Endpoint:
- `POST /api/ServiceCaseLine`

Örnek payload:

```json
{
  "serviceCaseId": 1,
  "lineType": 2,
  "processType": 1,
  "quantity": 1,
  "unit": "HIZMET",
  "erpOrderNo": "SIP-2026-145",
  "erpOrderId": "987654",
  "description": "Servis isciligi",
  "branchCode": "0"
}
```

Beklenen:
- `ServiceCaseLine` oluşur.
- `StockId` boş kalır.
- Allocation satırı oluşmaz.

## 4. Aynı Stok İçin Hakediş Dağıtımı

Amaç:
- Aynı stok için birden fazla sipariş satırında öncelik bazlı dağıtım çalışsın.

Ön koşul:
- Aynı `StockId` için en az 2 veya 3 allocation satırı olsun.

Endpoint:
- `POST /api/OrderAllocationLine/recompute`

Örnek payload:

```json
{
  "stockId": 7004,
  "availableQuantity": 7
}
```

Örnek veri varsayımı:
- Satır 1: `RequestedQuantity = 5`, `PriorityNo = 1`
- Satır 2: `RequestedQuantity = 10`, `PriorityNo = 2`
- Satır 3: `RequestedQuantity = 20`, `PriorityNo = 3`

Beklenen:
- Satır 1 `AllocatedQuantity = 5`, `Status = Allocated`
- Satır 2 `AllocatedQuantity = 2`, `Status = PartiallyAllocated`
- Satır 3 `AllocatedQuantity = 0`, `Status = Waiting`

## 5. Operasyon Sonrası Otomatik Tetikleme

Amaç:
- `WI`, `WT`, `WO`, `SH`, `SIT`, `SRT` tamamlandıktan sonra ilgili belgeye bağlı allocation satırları yeniden hesaplansın.
- Uygun vakalarda `BusinessDocumentLink` otomatik oluşsun.

Kapsam:
- `WI` = Intake
- `WT` = InternalTransfer
- `WO` = SparePartSupply
- `SH` = Shipment
- `SIT` = RepairOperation
- `SRT` = ReturnToMainWarehouse

Beklenen:
- Operasyon servisi commit sonrasında orkestrasyon çağırır.
- ERP stok bakiyesi okunur.
- Allocation recompute tetiklenir.
- İlgili `ServiceCase` için `BusinessDocumentLink` kaydı oluşur.

## 6. Timeline Doğrulama

Amaç:
- Kullanıcı ürünün hangi işlemle nereye gittiğini tek ekrandan görebilsin.

Endpoint:
- `GET /api/ServiceCase/{id}/timeline`

Örnek:
- `GET /api/ServiceCase/1/timeline`

Beklenen:
- `serviceCase`
- `lines`
- `timeline`

Timeline içinde şunlar görünmeli:
- belge modülü
- belge id
- link purpose
- linkedAt
- varsa from/to warehouse

## 7. Vaka Güncelleme

Amaç:
- Teşhis notu, durum ve depo bilgileri güncellenebilsin.

Endpoint:
- `PUT /api/ServiceCase/{id}`

Örnek payload:

```json
{
  "status": 5,
  "currentWarehouseId": 3,
  "diagnosisNote": "Ariza tespit edildi, parca degisimi basladi."
}
```

Beklenen:
- Vaka `InRepair` durumuna geçer.
- Güncel depo bilgisi kaydedilir.

## 8. Negatif Testler

Kontrol edilmesi gerekenler:
- `ServiceCase` için aynı `CaseNo` iki kez açılamamalı.
- `ServiceCaseLine` için `Quantity <= 0` kabul edilmemeli.
- `SparePart` ve `ReplacementProduct` satırlarında `StockId` zorunlu olmalı.
- `OrderAllocationLine/recompute` içinde negatif `AvailableQuantity` kabul edilmemeli.

## 9. Canlı Öncesi Kontrol Listesi

- Migration uygulandı mı?
- `ServiceCase` create/edit ekranı çalışıyor mu?
- İlk servis kalemi eklenince allocation satırı oluşuyor mu?
- Operasyon sonrası otomatik linkleme gerçekten çalışıyor mu?
- Timeline ekranında belge zinciri sıralı geliyor mu?
- Aynı stok için kısmi hakediş senaryosu beklenen sonucu veriyor mu?
- ERP stok bakiyesi okunamadığında sistem güvenli şekilde devam ediyor mu?
