# TESTING

## Quick start

Run all tests:

```powershell
dotnet test
```

Run only application/domain tests:

```powershell
dotnet test BladeVault.Application.Tests/BladeVault.Application.Tests.csproj
```

Run only API integration tests:

```powershell
dotnet test BladeVault.WebAPI.Tests/BladeVault.WebAPI.Tests.csproj
```

Run solution build check:

```powershell
dotnet build
```

## Test projects

- `BladeVault.Application.Tests`
  - Domain rules (`Order`, `Stock`)
  - Analytics handler unit tests

- `BladeVault.WebAPI.Tests`
  - Policy-based authorization tests (`403/200`)
  - E2E API flows (warehouse, stock, call center)

## Integration test auth model

`BladeVault.WebAPI.Tests` uses a test auth handler with request header:

- Header: `X-Test-Roles`
- Value examples: `Owner`, `Warehouse`, `CallCenter`, `Analyst`, `CatalogManager`

Example:

```http
X-Test-Roles: Warehouse
```

## Role access matrix (current policies)

| Policy | Allowed roles |
|---|---|
| `OwnerOrAdmin` | `Owner`, `Admin` |
| `ProductManagement` | `Owner`, `Admin`, `CatalogManager` |
| `OrderStatusManagement` | `Owner`, `Admin`, `CallCenter`, `Warehouse` |
| `StockManagement` | `Owner`, `Admin`, `CatalogManager` |
| `StockRead` | `Owner`, `Admin`, `CatalogManager`, `CallCenter`, `Warehouse` |
| `CallCenterOperations` | `Owner`, `Admin`, `CallCenter` |
| `WarehouseOperations` | `Owner`, `Admin`, `Warehouse` |
| `AnalyticsRead` | `Owner`, `Admin`, `Analyst` |

## API examples for manual smoke checks

### Analytics

`GET /api/analytics/dashboard?from=2026-03-01&to=2026-03-31`

Allowed roles: `Owner`, `Admin`, `Analyst`

### Stock

Change stock balance:

`POST /api/stock/change-balance`

Body:

```json
{
  "productId": "00000000-0000-0000-0000-000000000001",
  "delta": 10,
  "reason": "Restock",
  "documentReference": "DOC-101"
}
```

Get stock movements with filters + paging:

`GET /api/stock/{productId}/movements?movementType=Inbound&from=2026-03-01&to=2026-03-31&page=1&pageSize=20`

### Call Center

Create call log:

`POST /api/callcenter/logs`

Body:

```json
{
  "customerId": "00000000-0000-0000-0000-000000000010",
  "status": 3,
  "comment": "Client asked to call tomorrow",
  "nextCallAt": "2026-03-15T10:30:00Z"
}
```

Get logs with filters + paging:

`GET /api/callcenter/customers/{customerId}/logs?status=NeedCallback&from=2026-03-01&to=2026-03-31&page=1&pageSize=20`

### Warehouse

Get warehouse orders:

`GET /api/warehouse/orders?status=Confirmed&page=1&pageSize=20`

Start assembly:

`POST /api/warehouse/orders/{orderId}/start-assembly`

Mark ready to ship:

`POST /api/warehouse/orders/{orderId}/ready-to-ship`

Mark shipped (manual tracking or fallback to mock provider):

`POST /api/warehouse/orders/{orderId}/ship`

Body:

```json
{
  "trackingNumber": "NP-REAL-123456"
}
```

## Recommended local flow before commit

1. `dotnet test BladeVault.Application.Tests/BladeVault.Application.Tests.csproj`
2. `dotnet test BladeVault.WebAPI.Tests/BladeVault.WebAPI.Tests.csproj`
3. `dotnet build`

If all green, commit changes.
