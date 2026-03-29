# srcWms

`srcWms` is the new feature-first migration target for WMS backend code.

## Why this exists
The current codebase is module-based but physically split across four projects:
- `Wms.WebApi`
- `Wms.Application`
- `Wms.Domain`
- `Wms.Infrastructure`

That keeps boundaries clean, but it makes daily development harder because a single use-case is spread across multiple projects.

`srcWms` starts the next step: keep backend boundaries, but organize work around features first.

## Target shape
```text
srcWms/
  Wms/
    Shared/
      Api/
      Application/
      Domain/
      Infrastructure/
    Modules/
      GoodsReceipt/
        Api/
        Application/
        Domain/
        Infrastructure/
      WarehouseTransfer/
      WarehouseOutbound/
      WarehouseInbound/
      Shipping/
      Production/
      ProductionTransfer/
      SubcontractingIssueTransfer/
      SubcontractingReceiptTransfer/
      Package/
      Identity/
      Communications/
      Definitions/
```

## Migration strategy
1. Pick one module.
2. Move one complete vertical slice.
3. Keep routes and contracts stable.
4. Verify build and runtime.
5. Remove the old implementation only after the new slice is live.

## First pilot
The first pilot module is:
- `Wms/Modules/GoodsReceipt`

This lets us prove the new structure on the heaviest workflow before we move the other modules.
