# Reference Remove Readiness

Bu doküman eski code/string alanlarının kaldırılmasına ne kadar hazır olduğumuzu özetler.

## Güvenli Olanlar

Bu alanlar şu an id tabanlı akışa geçmiş durumda ama display/fallback olarak yaşamaya devam ediyor.
Henüz kaldırmak zorunda değiliz, fakat davranışsal sahiplikleri zayıfladı.

- Header DTO response yüzeyi:
  - `CustomerCode`
  - `SourceWarehouse`
  - `TargetWarehouse`
- Line DTO response yüzeyi:
  - `StockCode`
  - `YapKod`
- Route DTO response yüzeyi:
  - `SourceWarehouse`
  - `TargetWarehouse`

Not: Bunlar kullanıcıya görünür etiket ve fallback olarak kullanılıyor. Önce response contract versiyonlaması yapılmadan kaldırılmamalı.

## Henüz Güvenli Değil

### 1. Entity seviyesinde ana grouping alanları

Bu alanlar halen business logic içinde aktif kullanılıyor.

- `BaseLineEntity.StockCode`
- `BaseLineEntity.YapKod`
- `BaseImportLineEntity.StockCode`
- `BaseImportLineEntity.YapKod`
- `BaseRouteEntity.SourceWarehouse`
- `BaseRouteEntity.TargetWarehouse`

Sebep:
- grouping key oluşturma halen code bazlı
- route write modeli halen warehouse code/int alanlarına yazıyor
- validation ve matcher katmanında string/code karşılaştırmaları var

### 2. Header entity legacy alanları

Bu alanlar hâlâ modül bazlı request, response ve sorgu kontratlarında aktif.

- `CustomerCode`
- `SourceWarehouse`
- `TargetWarehouse`
- bazı modüllerde `WarehouseCode`
- production/package benzeri modüllerde `StockCode`, `YapKod`

Sebep:
- get-by-code ve open-order function mapping akışları sürüyor
- write path fallback için kullanılıyor
- UI list/detail filtrelerinde aktif

### 3. Package matcher ve benzeri core servisler

Aşağıdaki örnek alanlar hâlâ doğrudan code ile eşleşiyor:

- `PackageMatcherCore`
  - `StockCode`
  - `YapKod`

Sebep:
- mevcut matching kuralı string/code bazlı
- id tabanlı tam geçiş yapılmadı

## Remove Öncesi Mutlaka Refactor Gerekenler

### A. API domain/entity katmanı

Refactor edilmeden drop edilmemeli:

- `Shared/Domain/Entities/Common/BaseLineEntity.cs`
- `Shared/Domain/Entities/Common/BaseImportLineEntity.cs`
- `Shared/Domain/Entities/Common/BaseRouteEntity.cs`

Yapılması gereken:
- canonical sahipliği id alanlarına taşımak
- code alanlarını display/fallback DTO seviyesine indirmek

### B. API service katmanı

Refactor edilmeden drop edilmemeli:

- `GrHeaderService`
- `WoHeaderService`
- `WiHeaderService`
- `WtHeaderService`
- `ShHeaderService`
- `SitHeaderService`
- `SrtHeaderService`
- `PrHeaderService`
- `PtHeaderService`
- `PackageMatcherCore`

Yapılması gereken:
- grouping key: `LineId + StockId + YapKodId`
- line yoksa: `StockId + YapKodId`
- route warehouse write/read: warehouse id + warehouse name enrichment
- validation message fallback: code yerine relation display adı

### C. API DTO / function mapping katmanı

Refactor edilmeden drop edilmemeli:

- modül `*FunctionDtos.cs`
- modül `*HeaderDtos.cs`
- modül `*ChildDtos.cs`
- `Shared/Common/Application/Common/DocumentDtos.cs`

Yapılması gereken:
- request contract canonical alanları id yapmak
- string/code alanlarını optional display/read modeli olarak ayırmak

### D. Web / Mobile request builder katmanı

Refactor edilmeden drop edilmemeli:

- `document-models.ts`
- feature `types/*.ts`
- feature `utils/*generate.ts`
- mobile `api.ts` / `types.ts`

Yapılması gereken:
- canonical request alanlarını sadece id yapmak
- UI display için name/code alanlarını read-model’de tutmak

## Şu An Yapılmaması Gerekenler

Aşağıdakiler için remove migration erken olur:

- `StockCode` kolonlarını line/importline tablolarından kaldırmak
- `YapKod` kolonlarını line/importline tablolarından kaldırmak
- `SourceWarehouse` / `TargetWarehouse` kolonlarını route tablolarından kaldırmak
- `CustomerCode` kolonlarını tüm header tablolardan topluca kaldırmak

## Güvenli Sonraki Sıra

1. Grouping key’leri id tabanlı hale getir
2. Route warehouse sahipliğini id tabanlı hale getir
3. Package matcher ve benzeri code bazlı core servisleri dönüştür
4. Request contract’larda code alanlarını optional/fallback hale indir
5. Backfill ve null kontrol SQL’leri çalıştır
6. En son remove migration yaz

## Kısa Hüküm

Sistem artık id-first bir yöne girmiş durumda ama henüz code-free değil.
Şu an remove migration için hazır olunan yer response/display yüzeyi; hazır olunmayan yer ise domain ve service grouping omurgası.
