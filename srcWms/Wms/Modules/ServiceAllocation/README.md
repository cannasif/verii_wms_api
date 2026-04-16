# ServiceAllocation Module

`ServiceAllocation` is the proposed module for the combined service/repair and allocation workflow.

## Why this module exists

Current WMS modules already handle:

- warehouse inbound
- warehouse outbound
- warehouse transfer
- shipping
- subcontracting issue / receipt
- ERP line references

The missing business layer is the one that decides:

- which ERP order line (`ErpOrderId` = `inckeyNo`) deserves stock first
- how a repair/service case is opened and tracked
- which spare part and labor lines belong to the same service case
- how warehouse movements are linked back to a business case

## Core modeling decisions

1. `Header.OrderId` stays empty for multi-order documents.
2. The real ERP link is always line-based:
   - `ErpOrderNo` = ERP order number
   - `ErpOrderId` = ERP order `inckeyNo`
3. Allocation decisions must be made per line, not per header.
4. Service/repair should be tracked as a dedicated case that can reference many warehouse documents.

## Suggested aggregate roots

### 1. ServiceCase

Tracks the lifecycle of a broken product sent by the customer.

Suggested responsibilities:

- customer intake
- diagnosis tracking
- current warehouse ownership
- service status progression
- relation to created spare part / labor lines

### 2. OrderAllocationLine

Tracks entitlement/allocation decisions for each ERP order line.

Suggested responsibilities:

- line-level allocation queue
- partial allocation support
- shipment / reservation progression
- stock competition between many customers for the same item

### 3. BusinessDocumentLink

Links warehouse operations to the service case or allocation timeline.

Suggested responsibilities:

- connect `WI`, `WT`, `WO`, `SH`, `SIT`, `SRT` documents to a service case
- connect `WI`, `WT`, `WO`, `SH`, `SIT`, `SRT` documents to an allocation line
- record from/to warehouse context
- provide a single audit trail for the full repair journey

## Recommended implementation steps

1. Add line-level allocation state to the related operational line entities.
2. Create `OrderAllocationLine` as the business source of truth for entitlement decisions.
3. Create `ServiceCase` and `ServiceCaseLine` for broken product, spare part, and labor handling.
4. Create `BusinessDocumentLink` to build a full warehouse timeline without forcing new foreign key columns into operational tables.
5. Add application services that recompute allocation based on:
   - `StockCode`
   - `ErpOrderId`
   - priority sequence
   - available stock
6. Add a UI view that shows:
   - service case status
   - spare part and labor lines
   - linked ERP order references
   - linked warehouse movements
   - current warehouse / cell position

## Data rules

- A single header may contain lines from many ERP orders.
- Allocation must always be recalculated at line level.
- Service labor lines may not have a physical stock movement, but they still belong to the same case.
- Spare part lines may allocate from a different warehouse than the intake warehouse.
- The module should treat warehouse documents as operational evidence, not as the business case itself.

## MVP scope

- line-based allocation status model
- service case status model
- relation-based document link model
- timeline-ready enums and naming

## Next scope

- approval and exception rules
- blocked / waiting reason codes
- allocation recompute job
- service dashboard and audit screens
