# EnumToolkit for .NET

A simple but powerful toolkit for managing enums in .NET applications.

---

## What is this?

Enums are commonly used in applications to represent fixed sets of values — like `OrderStatus.Pending`, `OrderStatus.Completed`, etc.

This toolkit helps you:

- 🌍 Show enum values in **multiple languages** (like English or Turkish)
- 🌐 Access enum values via a **REST API** for use in frontends (dropdowns, filters, etc.)
- 🧱 (Optionally) **store enums in a database** so UIs and external systems can read them
- 🔧 Easily register everything with one line in `Program.cs`

---

## Example Scenario

Imagine you have this enum:

```csharp
public enum OrderStatus
{
    [LocalizedDescription("en", "Pending")]
    [LocalizedDescription("tr", "Bekliyor")]
    Pending = 0,

    [LocalizedDescription("en", "Completed")]
    [LocalizedDescription("tr", "Tamamlandı")]
    Completed = 1
}

```
### With this toolkit, you can:

* Show "Bekliyor" or "Pending" automatically depending on the user’s language
* Return the enum list from an API like:
```csharp
[
  { "name": "Pending", "value": 0, "description": "Bekliyor" },
  { "name": "Completed", "value": 1, "description": "Tamamlandı" }
]

```
## Features

- ✅ Get enum descriptions in multiple languages  
- ✅ List all enums at runtime via API  
- ✅ Convert enums to DTOs easily  
- ✅ Sync enums to database (`EnumTypeLookup`)  
- ✅ Plug-and-play dependency injection support  

---

## Installation

### 1. Add package reference to your project (if distributed via NuGet or local).
### 2. Register services and scan enums in `Program.cs`:
```csharp
builder.Services.AddEnumServices(typeof(SomeEnum).Assembly);

```
### 3. (Optional) Add minimal API endpoints:
```csharp
app.MapEnumEndpoints();

// This will expose two endpoints:
// GET /Enums/types — list available enum types
// GET /Enums?enumName=YourEnumName&language=en — get enum values and localized descriptions

```
## Basic Usage
### Get Enum Description
Descriptions are retrieved from [LocalizedDescription("en", "Completed")] attributes if present.
```csharp
var status = OrderStatus.Completed;
var description = status.GetDescription("en"); // e.g., "Completed"

```
### Get Enum as DTO List
```csharp
var values = EnumExtensions.ToEnumValueDtoList<OrderStatus>("tr");
// Returns List<EnumValueDto> with Name, Value, and Description

```
### Injected IEnumService (with culture awareness)
```csharp
public class MyService
{
    private readonly IEnumService _enumService;

    public MyService(IEnumService enumService)
    {
        _enumService = enumService;
    }

    public List<EnumValueDto> Get(string name)
        => _enumService.GetEnumValues(name); // Uses current user's language
}

```
### Optional: Sync to Database
You can sync enums to a database table (EnumTypeLookup) to allow runtime usage in UI or other systems.
```csharp
var syncService = new EnumSyncService(enumMetadataList);
await syncService.SyncAllAsync(dbContext, new[] { "en", "tr" });
```
* This will insert or update records for all known enums and languages.

### Enum Attribute Example
```csharp
public enum OrderStatus
{
    [LocalizedDescription("en", "Pending")]
    [LocalizedDescription("tr", "Bekliyor")]
    Pending = 0,

    [LocalizedDescription("en", "Completed")]
    [LocalizedDescription("tr", "Tamamlandı")]
    Completed = 1
}

```
### Output DTO Format
```csharp
[
  {
    "name": "Pending",
    "value": 0,
    "description": "Bekliyor"
  },
  {
    "name": "Completed",
    "value": 1,
    "description": "Tamamlandı"
  }
]

```
## Contribution
```bash
1. Fork the repository
2. Create your feature branch (git checkout -b feature/enum-improvements)
3. Commit your changes (git commit -m "Add fallback logic for neutral culture")
4. Push to the branch (git push origin feature/enum-improvements)
5. Open a Pull Request

```
## License
```bash
MIT — Free for personal and commercial use.
