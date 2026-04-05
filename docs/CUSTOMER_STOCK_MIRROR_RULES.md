# Customer / Stock Mirror Rules

## CRM source tables
- `RII_CUSTOMER`
- `RII_STOCK`

## WMS mirror tables
- `RII_WMS_CUSTOMER`
- `RII_WMS_STOCK`

## WMS mirror approach
- ERP remains source of truth.
- WMS keeps an operational mirror for paging, search, selection and workflow support.
- Mirror tables are not treated as master data owners.

## Customer mirror fields kept in WMS
- `CustomerCode`
- `CustomerName`
- `TaxOffice`
- `TaxNumber`
- `TcknNumber`
- `SalesRepCode`
- `GroupCode`
- `CreditLimit`
- `BranchCode`
- `BusinessUnitCode`
- `Email`
- `Website`
- `Phone1`
- `Phone2`
- `Address`
- `City`
- `District`
- `CountryCode`
- `IsErpIntegrated`
- `ErpIntegrationNumber`
- `LastSyncDate`

## Stock mirror fields kept in WMS
- `ErpStockCode`
- `StockName`
- `Unit`
- `UreticiKodu`
- `GrupKodu`
- `GrupAdi`
- `Kod1` / `Kod1Adi`
- `Kod2` / `Kod2Adi`
- `Kod3` / `Kod3Adi`
- `Kod4` / `Kod4Adi`
- `Kod5` / `Kod5Adi`
- `BranchCode`
- `LastSyncDate`

## Sync rules
- `CustomerSyncAsync` matches by `CustomerCode`.
- `StockSyncAsync` matches by `ErpStockCode`.
- Existing records are updated.
- Missing records are inserted.
- Soft-deleted records are revived on sync.
- `LastSyncDate` is refreshed on every synced row.

## Why this shape
- Keeps WMS fast and operationally independent for reads.
- Avoids coupling list/filter/search flows directly to CRM.
- Gives Hangfire a stable upsert target later.
