# Production API Examples

## `POST /api/PrHeader/plan`

```json
{
  "source": "manual",
  "header": {
    "documentNo": "PR-PLAN-0001",
    "documentDate": "2026-04-06",
    "description": "Kalem uretim plani",
    "executionMode": "Serial",
    "planType": "Production",
    "priority": 1,
    "projectCode": "PRJ-01",
    "customerCode": "CARI001",
    "mainStockCode": "KALEM",
    "mainYapKod": "STD",
    "plannedQuantity": 3,
    "plannedStartDate": "2026-04-07",
    "plannedEndDate": "2026-04-07",
    "assignments": [
      {
        "localId": "header-assignment-1",
        "assignedUserId": 12,
        "assignmentType": "Primary"
      }
    ]
  },
  "orders": [
    {
      "localId": "order-1",
      "orderNo": "EMIR01",
      "orderType": "Production",
      "producedStockCode": "KALEM",
      "producedYapKod": "STD",
      "plannedQuantity": 3,
      "sourceWarehouseCode": "10",
      "targetWarehouseCode": "20",
      "sequenceNo": 1,
      "canStartManually": false,
      "autoStartWhenDependenciesDone": false,
      "assignments": [
        {
          "localId": "order-assignment-1",
          "assignedUserId": 25,
          "assignmentType": "Primary",
          "note": "Sabah vardiyasi"
        }
      ]
    }
  ],
  "outputs": [
    {
      "localId": "output-1",
      "orderLocalId": "order-1",
      "stockCode": "KALEM",
      "yapKod": "STD",
      "plannedQuantity": 3,
      "unit": "ADET",
      "trackingMode": "Serial",
      "serialEntryMode": "Optional",
      "targetWarehouseCode": "20",
      "targetCellCode": "A-01"
    }
  ],
  "consumptions": [
    {
      "localId": "cons-1",
      "orderLocalId": "order-1",
      "stockCode": "UC",
      "yapKod": "",
      "plannedQuantity": 3,
      "unit": "ADET",
      "trackingMode": "Serial",
      "serialEntryMode": "Required",
      "sourceWarehouseCode": "10",
      "sourceCellCode": "HAM-01",
      "isBackflush": false,
      "isMandatory": true
    }
  ],
  "dependencies": []
}
```

## `POST /api/PrHeader/erp-template`

```json
{
  "orderNo": "EMIR01",
  "stockCode": "KALEM",
  "yapKod": "STD",
  "quantity": 3
}
```

## `POST /api/PtHeader/production-transfer`

```json
{
  "documentNo": "PT-PR-0001",
  "documentDate": "2026-04-06",
  "transferPurpose": "MaterialSupply",
  "productionDocumentNo": "PR-PLAN-0001",
  "productionOrderNo": "EMIR01",
  "sourceWarehouseCode": "10",
  "targetWarehouseCode": "30",
  "description": "Uretim besleme transferi",
  "lines": [
    {
      "localId": "pt-line-1",
      "stockCode": "UC",
      "yapKod": "",
      "quantity": 3,
      "lineRole": "ConsumptionSupply",
      "sourceCellCode": "HAM-01",
      "targetCellCode": "BES-01",
      "productionOrderNo": "EMIR01"
    }
  ]
}
```

## `POST /api/PrOperation/start`

```json
{
  "orderId": 101,
  "operationType": "ProductionRun",
  "workcenterId": 5,
  "machineId": 3,
  "plannedDurationMinutes": 120,
  "description": "Sabah vardiyasi baslatildi"
}
```

## `POST /api/PrOperation/{operationId}/consumption`

```json
{
  "stockCode": "UC",
  "quantity": 1,
  "unit": "ADET",
  "serialNo1": "UC-0001",
  "sourceWarehouseCode": "10",
  "sourceCellCode": "HAM-01",
  "scannedBarcode": "UC///UC-0001"
}
```

## `POST /api/PrOperation/{operationId}/output`

```json
{
  "stockCode": "KALEM",
  "quantity": 1,
  "unit": "ADET",
  "serialNo1": "KLM-0001",
  "targetWarehouseCode": "20",
  "targetCellCode": "A-01"
}
```
