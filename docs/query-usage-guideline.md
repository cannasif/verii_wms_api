# Query Usage Guideline

This project uses `Query(bool tracking = false, bool ignoreQueryFilters = false)` as the standard repository entry point.

## Default rule
- Use `Query()` for all read-only list, detail, search, report, and paged queries.
- `Query()` applies `AsNoTracking()` and the soft-delete filter by default.

## When to use tracking
- Use `Query(tracking: true)` when the loaded entity will be updated, approved, completed, soft-deleted, or otherwise mutated in the same flow.
- Typical examples:
  - `UpdateAsync`
  - `SetApprovalAsync`
  - `CompleteAsync`
  - write-side validation that mutates the same entity later

## When to ignore query filters
- Use `Query(ignoreQueryFilters: true)` only when a flow must explicitly see soft-deleted rows.
- This should be rare and should be justified in code.

## What to avoid
- Avoid `AsQueryable()` directly on repositories in service code.
- Avoid `FindAsync(...)` for normal read flows when the same logic can be expressed with `Query().Where(...)`.
- Avoid `GetByIdAsync(...)` inside service methods for read flows; prefer `Query().Where(x => x.Id == id).FirstOrDefaultAsync()` so the tracking/filter intent is visible.

## Practical patterns

### Read-only detail
```csharp
var entity = await _unitOfWork.WtHeaders.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
```

### Write flow
```csharp
var entity = await _unitOfWork.WtHeaders.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync();
```

### Paged/list flow
```csharp
var query = _unitOfWork.WtHeaders.Query()
    .Where(x => x.IsCompleted);
```

### Subquery / EXISTS style checks
When expression trees are involved, use explicit arguments if needed:
```csharp
_unitOfWork.WtTerminalLines.Query(false, false)
```
This avoids optional-argument issues in some LINQ expression scenarios.

## Team rule of thumb
- Read path: `Query()`
- Write path: `Query(tracking: true)`
- Deleted data inspection: `Query(ignoreQueryFilters: true)`
