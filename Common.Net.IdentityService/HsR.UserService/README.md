# HsR User Service - Core Library

A minimal identity-based user service **core library** for the AssetsFlow trading journal system. This library contains pure business logic without any transport layer dependencies.

## Architecture

This is structured as a **two-project solution** located in `Common.Net/HsR.UserService/`:

### 1. **HsR.UserService** (Core Library - DLL)
- **Type**: Class Library (`Microsoft.NET.Sdk`)
- **Output**: `HsR.UserService.dll`
- **Purpose**: Pure business logic, no transport dependencies
- **Cannot run** - Just contains reusable logic

### 2. **HsR.UserService.Host** (Host Application)
- **Type**: Web Application (`Microsoft.NET.Sdk.Web`)
- **Output**: Executable web server
- **Purpose**: Hosts the service and provides transport layer
- **Can run** - This is what you actually execute

## Project Structure

```
Common.Net/HsR.UserService/
├── HsR.UserService/ (Core Library - DLL)
│   ├── Services/
│   │   └── UserService.cs             # Authentication business logic
│   ├── Entities/
│   │   └── User.cs                    # User entity with Identity
│   ├── Models/
│   │   └── AuthModels.cs              # DTOs for authentication
│   ├── Data/
│   │   └── UserDbContext.cs           # Entity Framework context
│   └── HsR.UserService.csproj         # Library project
│
└── HsR.UserService.Host/ (Host Application)
    ├── Program.cs                     # Application startup
    ├── appsettings.json               # Configuration
    └── HsR.UserService.Host.csproj    # Web project
```

## Current State

### ✅ **What's Implemented:**
- **Core business logic** in library project
- **Identity setup** with PostgreSQL
- **User registration and login** services
- **Basic host application** (ready for gRPC)

### ❌ **What's Not Implemented Yet:**
- **HTTP controllers** (removed for clean structure)
- **gRPC services** (future implementation)
- **API endpoints** (will be added in host application)
- **Demo user functionality** (removed for minimal approach)

## How It Works

### Runtime Flow:
```
┌─────────────────────────────────────┐
│ HsR.UserService.Host                │ ← Host Application (Runs)
│ ├── Program.cs                      │ ← Entry point
│ └── References HsR.UserService      │ ← Uses the DLL
└─────────────────────────────────────┘
           │
           ▼
┌─────────────────────────────────────┐
│ HsR.UserService.dll                 │ ← Library (Loaded)
│ ├── UserService.cs                  │ ← Business logic
│ └── UserDbContext.cs                │ ← Data access
└─────────────────────────────────────┘
```

## Benefits of This Structure

### ✅ **Clean Separation:**
- **Business logic** is separate from **hosting concerns**
- **Core library** can be reused by different host applications
- **Easy to test** business logic without hosting dependencies

### ✅ **Structured Development:**
- **Phase 1**: Core business logic ✅ (current)
- **Phase 2**: Add gRPC services to host (future)
- **Phase 3**: Integration and testing

### ✅ **Flexibility:**
- Can add **HTTP**, **gRPC**, or **both** transport layers
- Core logic remains **unchanged** regardless of transport
- **Multiple host applications** can use the same core logic

## Running the Service

### Current State (No Transport Layer Yet):
```bash
cd Common.Net/HsR.UserService/HsR.UserService.Host
dotnet run
# Currently runs but has no endpoints (transport layer not implemented)
```

### Future State (gRPC Transport):
```bash
cd Common.Net/HsR.UserService/HsR.UserService.Host
dotnet run
# Will expose gRPC endpoints when implemented
```

## Development Workflow

### 1. **Core Logic Development:**
- Work in `Common.Net/HsR.UserService/HsR.UserService/` (the library)
- Write business logic, entities, services
- Unit test the core logic

### 2. **Host Application Development:**
- Work in `Common.Net/HsR.UserService/HsR.UserService.Host/` (the host)
- Add gRPC services, controllers, etc.
- Integration test the full service

### 3. **Integration:**
- Host application references core library
- Host application calls core services
- Core library has no knowledge of hosting

## Core Services Available

### UserService
```csharp
namespace HsR.UserService.Services
{
    public class UserService
    {
        public async Task<AuthResponse> LoginAsync(LoginRequest request);
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request);
    }
}
```

## Configuration

The host application uses `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AssetsFlow_Users;Username=postgres;Password=your_password;"
  }
}
```

## Integration with AssetsFlow.Server

The UserService projects are now located in `Common.Net/HsR.UserService/` and can be referenced by the main AssetsFlow.Server solution. This provides:

- **Shared location** with other common .NET libraries
- **Independent development** from the main server
- **Reusable** across multiple projects
- **Clean separation** of concerns

This structure gives you the **best of both worlds**: clean, testable business logic and flexible hosting options! The core library is now truly minimal with only essential authentication services. 