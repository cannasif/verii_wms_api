# Warehouse Transaction Rules

This document is the source of truth for warehouse transaction modeling in WMS.

It applies to flows such as:
- goods receipt
- warehouse inbound
- warehouse outbound
- warehouse transfer
- shipment
- similar line-based or free warehouse operations

The goal is simple:
- every field must have one semantic owner
- DTOs, request builders, services, and entities must follow the same ownership
- no layer should guess where a field belongs

## Core Structure

Always model warehouse transactions with this structure:

1. `Header`
2. `Line`
3. `LineSerial`
4. `ImportLine`
5. `Route`

This rule uses `ImportLine -> Route` as the collected-product model.

## Ownership Rules

### 1. Header

`Header` is the transaction's main document.

It owns:
- document identity
- branch
- customer
- project
- document number and date
- transaction type
- high-level warehouse transaction metadata
- approval / completion state
- top-level notes

Think of `Header` as:
- "this warehouse transaction exists"
- "who owns this transaction"

`Header` must not store:
- requested item breakdown
- serial-level request details
- collected movement details

### 2. Line

`Line` is the order / work instruction line.

It owns:
- order linkage
- order line linkage
- requested stock
- requested unit
- requested configuration such as `YapKod`
- order quantity
- if order information exists, `SiparisMiktar`

Think of `Line` as:
- "what is requested"
- "which order line this request belongs to"

`Line` must not store:
- collected route history
- physical movement details
- per-scan warehouse/cell trace

### 3. LineSerial

`LineSerial` is the requested serial/detail breakdown of the `Line`.

It owns:
- requested quantity at serial/detail level
- serial information
- lot / batch style detail when this belongs to the request
- warehouse information if request semantics require it
- cell / raf information if request semantics require it

Think of `LineSerial` as:
- "how the requested line is broken down in detail"

`LineSerial` must not be the collected movement owner.

### 4. ImportLine

`ImportLine` is the collected/processed product group.

It owns:
- the collected product grouping
- optional linkage to `Line`
- optional linkage to `LineSerial`
- the grouping boundary of collected work that will have `Route` details under it

Think of `ImportLine` as:
- "the collected product group"
- "the work result bucket that routes belong to"

#### ImportLine grouping rules

##### If `Line` exists

`ImportLine` must be grouped according to the request context:
- by `Line`
- by `LineSerial` quantity semantics
- and if needed by serial semantics

Meaning:
- collected work tied to a requested line must not be merged across unrelated requested lines
- if line-serial detail creates separate requested buckets, `ImportLine` must respect that boundary

##### If `Line` does not exist

When there is no order / no request line:
- there must be one `ImportLine` for one `Stock + YapKod`

Meaning:
- if `LineId` is null, the collected grouping owner is `Stock + YapKod`
- all collected route records for the same `Stock + YapKod` go under the same `ImportLine`

This is the non-negotiable free-operation rule.

### 5. Route

`Route` is the physical movement detail of an `ImportLine`.

It owns:
- quantity
- serial
- lot / batch / extra serial fields when applicable
- warehouse
- cell / raf
- barcode if movement detail is barcode-based
- route-level movement trace

Think of `Route` as:
- "one collected physical movement detail"

`Route` is always subordinate to `ImportLine`.

The semantic sentence is:
- `ImportLine` = collected grouping owner
- `Route` = one physical detail of that collected grouping

## Required Semantic Sentences

Every warehouse implementation should satisfy these sentences:

- `Header` = transaction main info
- `Line` = order/request info and `SiparisMiktar` when order exists
- `LineSerial` = requested detail and requested quantity breakdown
- `ImportLine` = collected grouping owner
- `Route` = physical detail tied to `ImportLine`

If a field does not fit these sentences, it probably belongs to the wrong entity.

## Practical Mapping Rules

### Requested data goes here

- order references -> `Line`
- order quantity -> `Line`
- `SiparisMiktar` -> `Line`
- requested stock -> `Line`
- requested configuration -> `Line`
- requested serial/detail quantity -> `LineSerial`
- requested warehouse/cell/serial detail -> `LineSerial`

### Collected grouping goes here

- collected group with line context -> `ImportLine`
- collected group with line-serial context -> `ImportLine`
- collected group without line context -> one `ImportLine` per `Stock + YapKod`

### Physical movement data goes here

- moved quantity -> `Route`
- scanned barcode -> `Route`
- serial -> `Route`
- lot / batch -> `Route`
- warehouse -> `Route`
- cell / raf -> `Route`

## Non-Negotiable Rules

### Rule 1

Do not use `Line` as if it is the collected movement record.

Correct:
- request lives in `Line`
- request detail lives in `LineSerial`
- collected grouping lives in `ImportLine`
- movement detail lives in `Route`

### Rule 2

Do not merge different requested line buckets into one `ImportLine`.

If `Line` exists:
- `ImportLine` must respect line boundaries
- and when needed, line-serial / serial boundaries

### Rule 3

If `Line` does not exist, do not create multiple `ImportLine` rows for the same `Stock + YapKod`.

Correct:
- one `ImportLine`
- many `Route` rows under it

### Rule 4

Do not store physical movement semantics in `ImportLine` if the data belongs to `Route`.

Bad:
- putting all serial, raf, warehouse, and quantity detail only on `ImportLine`

Correct:
- grouping owner = `ImportLine`
- physical detail = `Route`

### Rule 5

Do not hide canonical fields inside free text fields like `Description`, `Description1`, `Description2`.

Correct:
- quantity in quantity fields
- serial in serial fields
- warehouse/cell in warehouse/cell fields

### Rule 6

UI, DTO, and service layers must preserve this ownership.

This means:
- web/mobile builders must send data according to this model
- services must create `ImportLine` and `Route` with the same grouping rule
- no layer should invent a different grouping rule

## Implementation Notes

From this point forward, the intended business rule is:
- collected grouping owner = `ImportLine`
- physical movement owner = `Route`

Any future refactor, create/update flow, or sync logic must follow this rule.
