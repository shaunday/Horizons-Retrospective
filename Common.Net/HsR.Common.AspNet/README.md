# HsR.Common.Asp.NET

Reusable ASP.NET Core extension methods for Horizons-Retrospective microservices.

## Purpose
This library provides common ASP.NET Core helpers, such as:
- Database initialization for any `DbContext`
- (Future) Common middleware, logging, or configuration helpers

## Key Extension
### Ensure Database Created
```csharp
await app.EnsureDatabaseCreatedAsync<MyDbContext>();
```
- Ensures the database for the given `DbContext` is created on startup.
- Usage: Call in your `Program.cs` after building the app.

## Usage
1. Add a project reference to `Common.Net/HsR.Common.Asp.NET` in your web or service project.
2. Add `using HsR.Common.AspNet;` in your `Program.cs`.
3. Call the extension method as needed.

## Requirements
- .NET 9.0 SDK (Preview)
- Microsoft.EntityFrameworkCore
- Microsoft.NET.Sdk.Web (as SDK)

## Example
```csharp
using HsR.Common.AspNet;

var app = builder.Build();
await app.EnsureDatabaseCreatedAsync<MyDbContext>();
```

## Notes
- This project uses the Web SDK but is a class library (`<OutputType>Library</OutputType>`)
- Do not add direct references to `Microsoft.AspNetCore.App.Ref` in class libraries
- All code is nullable-enabled and modern C# 