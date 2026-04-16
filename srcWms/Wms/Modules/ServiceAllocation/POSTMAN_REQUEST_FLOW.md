# ServiceAllocation Postman Request Flow

Bu doküman Postman collection mantığında hazırlanmış örnek request akışını içerir.

Önerilen collection adı:
- `ServiceAllocation E2E`

Önerilen variable'lar:

```text
baseUrl = https://your-api-url
token = Bearer <jwt-token>
serviceCaseId = 1
erpOrderId = 987654
erpOrderNo = SIP-2026-145
incomingStockId = 5001
partStockId = 7004
```

Önerilen header:

```text
Authorization: {{token}}
Content-Type: application/json
```

## Folder 1 - Service Case Setup

### 1. Create Service Case

Request:
- `POST {{baseUrl}}/api/ServiceCase`

Body:

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
  "diagnosisNote": "Ilk kabul yapildi.",
  "status": 2,
  "receivedAt": "2026-04-14",
  "branchCode": "0"
}
```

Postman test script önerisi:

```javascript
pm.test("Service case created", function () {
  pm.response.to.have.status(200);
});

const json = pm.response.json();
pm.collectionVariables.set("serviceCaseId", json.data.id);
```

### 2. Get Service Case

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}`

Beklenen:
- vaka kaydı dönmeli

## Folder 2 - Service Case Lines

### 3. Add Spare Part Line

Request:
- `POST {{baseUrl}}/api/ServiceCaseLine`

Body:

```json
{
  "serviceCaseId": {{serviceCaseId}},
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
- servis kalemi oluşmalı
- ilgili allocation satırı otomatik oluşmalı

### 4. Add Labor Line

Request:
- `POST {{baseUrl}}/api/ServiceCaseLine`

Body:

```json
{
  "serviceCaseId": {{serviceCaseId}},
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
- servis kalemi oluşmalı
- allocation oluşmamalı

## Folder 3 - Allocation Checks

### 5. Get Allocation Lines By ERP Order

Request:
- `POST {{baseUrl}}/api/OrderAllocationLine/paged`

Body:

```json
{
  "pageNumber": 1,
  "pageSize": 20,
  "filters": [
    {
      "field": "ErpOrderId",
      "operator": "eq",
      "value": "{{erpOrderId}}"
    }
  ]
}
```

Beklenen:
- en az bir allocation satırı dönmeli

### 6. Manual Recompute

Request:
- `POST {{baseUrl}}/api/OrderAllocationLine/recompute`

Body:

```json
{
  "stockId": {{partStockId}},
  "availableQuantity": 7
}
```

Beklenen:
- `processedLineCount > 0`
- `allocatedQuantity` değerleri güncellenmiş olmalı

## Folder 4 - Timeline Checks

### 7. Get Service Case Timeline

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- `serviceCase`
- `lines`
- `timeline`

Kontrol:
- servis kalemleri görünmeli
- sonraki operasyonlar işlendikçe timeline büyümeli

### 8. Get Business Links By Service Case

Request:
- `GET {{baseUrl}}/api/BusinessDocumentLink/by-entity?businessEntityId={{serviceCaseId}}&businessEntityType=0`

Not:
- `businessEntityType=0` => `ServiceCase`

Beklenen:
- ilgili service case için tüm belge linkleri dönmeli

## Folder 5 - Service Case Update

### 9. Update Service Case Status

Request:
- `PUT {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}`

Body:

```json
{
  "status": 5,
  "currentWarehouseId": 3,
  "diagnosisNote": "Ariza tespit edildi, parca degisimi basladi."
}
```

Beklenen:
- vaka `InRepair` benzeri duruma geçmeli

## Folder 6 - Post Operation Validation

Bu klasörde gerçek operasyon tamamlandıktan sonra kontrol request'leri çalıştırılır.

### 10. Validate After WI

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- timeline içinde `WI`
- `LinkPurpose = Intake`

### 11. Validate After WT

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- timeline içinde `WT`
- `LinkPurpose = InternalTransfer`

### 12. Validate After WO

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- timeline içinde `WO`
- `LinkPurpose = SparePartSupply`

### 13. Validate After SH

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- timeline içinde `SH`
- `LinkPurpose = Shipment`

### 14. Validate After SIT

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- timeline içinde `SIT`
- `LinkPurpose = RepairOperation`

### 15. Validate After SRT

Request:
- `GET {{baseUrl}}/api/ServiceCase/{{serviceCaseId}}/timeline`

Beklenen:
- timeline içinde `SRT`
- `LinkPurpose = ReturnToMainWarehouse`

## Folder 7 - Negative Tests

### 16. Duplicate CaseNo

Request:
- `POST {{baseUrl}}/api/ServiceCase`

Body:
- ilk request ile aynı `caseNo`

Beklenen:
- hata dönmeli

### 17. Spare Part Without StockId

Request:
- `POST {{baseUrl}}/api/ServiceCaseLine`

Body:

```json
{
  "serviceCaseId": {{serviceCaseId}},
  "lineType": 1,
  "processType": 1,
  "quantity": 1,
  "erpOrderNo": "{{erpOrderNo}}",
  "erpOrderId": "{{erpOrderId}}",
  "branchCode": "0"
}
```

Beklenen:
- validasyon hatası dönmeli

### 18. Recompute With Negative Quantity

Request:
- `POST {{baseUrl}}/api/OrderAllocationLine/recompute`

Body:

```json
{
  "stockId": {{partStockId}},
  "availableQuantity": -1
}
```

Beklenen:
- validasyon hatası dönmeli

## Önerilen Çalışma Sırası

1. `Create Service Case`
2. `Add Spare Part Line`
3. `Add Labor Line`
4. `Get Allocation Lines By ERP Order`
5. `Manual Recompute`
6. `Get Service Case Timeline`
7. gerçek operasyonları sırayla çalıştır
8. `Validate After WI/WT/WO/SH/SIT/SRT`

## Başarı Kriteri

- servis vakası düzgün oluşuyor mu
- kalemler doğru kaydoluyor mu
- yedek parça satırı allocation üretiyor mu
- işçilik satırı allocation üretmiyor mu
- recompute doğru dağıtıyor mu
- operasyon sonrası linkler otomatik oluşuyor mu
- timeline kullanıcıya anlamlı akıyor mu
