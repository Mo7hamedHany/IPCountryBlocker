# 🌐 IP Country Blocker API

<div align="center">

**A high-performance, enterprise-grade .NET 8 Web API for intelligent IP geolocation and country-based access control.**

[Features](#-features) • [Quick Start](#-quick-start) • [API Documentation](#-api-documentation) • [Architecture](#-architecture) • [Contributing](#-contributing)

</div>

---

## 📖 Overview

IP Country Blocker is a comprehensive solution for managing geographic access control to your applications. Built with modern .NET 8, it provides real-time IP geolocation, flexible country blocking mechanisms, and detailed attempt logging—all backed by high-speed Redis storage.

### 🎯 Key Highlights

- **⚡ Lightning Fast** - Redis-powered in-memory storage for sub-millisecond response times
- **🔒 Flexible Blocking** - Permanent and temporary (time-based) country blocking
- **🌍 Accurate Geolocation** - Integration with multiple IP geolocation providers
- **📊 Comprehensive Logging** - Track and analyze all blocking attempts
- **🌐 Multi-Language** - Built-in localization for 4 languages
- **🏗️ Clean Architecture** - CQRS pattern with MediatR for maintainability
- **✅ Production Ready** - Extensive test coverage and error handling

---

## 🚀 Features

### 🛡️ Core Functionality

#### 1. **Permanent Country Blocking**
- Block countries indefinitely from accessing your application
- ISO 3166-1 alpha-2 country code validation (e.g., `US`, `GB`, `EG`)
- Duplicate prevention with automatic validation
- Bulk operations support for managing multiple countries
- Thread-safe operations with Redis atomic commands

#### 2. **Temporary Country Blocking**
- Time-based blocking from 1 minute to 24 hours (1–1440 minutes)
- Automatic expiration with background cleanup
- Perfect for rate limiting or temporary restrictions
- Prevents duplicate temporal blocks
- Redis TTL for efficient memory management

#### 3. **IP Geolocation Intelligence**
- **Comprehensive Data**: Country code, name, city, ISP, organization, coordinates
- **Multiple Providers**: ipapi.co and IPGeolocation.io with automatic fallback
- **Protocol Support**: Both IPv4 and IPv6 addresses
- **Proxy Aware**: Handles `X-Forwarded-For` and `X-Real-IP` headers
- **Smart Defaults**: Falls back to caller's IP when not specified
- **Rate Limiting**: Built-in protection against API quota exhaustion

#### 4. **Real-Time Block Checking**
- Instant validation of incoming requests
- Automatic IP-to-country resolution
- Cross-reference with blocked country list
- Detailed response with blocking reason
- Automatic attempt logging for auditing

#### 5. **Advanced Attempt Logging**
```
📋 Log Entry Includes:
  • IP Address & Country Code
  • Timestamp (UTC)
  • User Agent String
  • Block Status (Blocked/Allowed)
  • Geographic Details
```

- **Pagination**: Handle millions of logs efficiently
- **Filtering**: Search by IP address or country code
- **Retention**: Auto-cleanup after 30 days (configurable)
- **Performance**: Redis sorted sets for O(log N) queries

### 🔧 Technical Features

#### Data Layer
- **Redis Integration**: StackExchange.Redis for high-performance caching
- **Atomic Operations**: Thread-safe operations with Redis transactions
- **TTL Support**: Automatic expiration for temporary data
- **JSON Serialization**: Newtonsoft.Json for complex object storage

#### Validation & Security
- **FluentValidation**: Declarative validation rules
- **Input Sanitization**: Prevent injection attacks
- **Rate Limiting**: Protect against abuse
- **Error Handling**: Global exception middleware

#### Background Processing
- **Hangfire Integration**: Reliable background job processing
- **Recurring Jobs**: Scheduled cleanup every 5 minutes
- **Dashboard**: Monitor jobs at `/hangfire`
- **Persistence**: Survives application restarts

#### Localization
| Language | Code | Coverage |
|----------|------|----------|
| English | `en-US` | ✅ 100% |
| German | `de-DE` | ✅ 100% |
| French | `fr-FR` | ✅ 100% |
| Arabic | `ar-EG` | ✅ 100% |

**Usage**: 
- Query parameter: `?culture=ar-EG`
- Header: `Accept-Language: de-DE`

#### API Documentation
- **OpenAPI/Swagger**: Interactive API documentation
- **Try It Out**: Test endpoints directly in browser
- **Schema Definitions**: Complete request/response models
- **Access**: Available at `/swagger`

---

## 🏗️ Architecture

### 📁 Project Structure

```
IPCountryBlocker/
│
├── 📦 IPCountryBlocker.Domain/
│   ├── Models/                    # Domain entities
│   │   ├── BlockedCountry.cs
│   │   ├── IpLookupResult.cs
│   │   └── BlockedAttemptLog.cs
│   ├── Interfaces/                # Repository contracts
│   │   ├── IBlockedCountryRepository.cs
│   │   └── IBlockedAttemptLogRepository.cs
│   └── DTOs/                      # Data transfer objects
│
├── 🎯 IPCountryBlocker.Application/
│   ├── Features/                  # CQRS Commands & Queries
│   │   ├── Countries/
│   │   │   ├── Commands/
│   │   │   │   ├── BlockCountryCommand.cs
│   │   │   │   ├── UnblockCountryCommand.cs
│   │   │   │   └── BlockCountryTemporaryCommand.cs
│   │   │   └── Queries/
│   │   │       └── GetBlockedCountriesQuery.cs
│   │   ├── IpLookup/
│   │   └── BlockChecking/
│   ├── Validators/                # FluentValidation rules
│   ├── Bases/                     # Base response handlers
│   └── Resources/                 # Localization files
│
├── 🔌 IPCountryBlocker.Infrastructure/
│   ├── Repositories/              # Redis implementations
│   │   ├── BlockedCountryRepository.cs
│   │   └── BlockedAttemptLogRepository.cs
│   ├── Specifications/            # Query specifications
│   └── Data/
│       └── RedisContext.cs
│
├── 🎨 IPCountryBlocker.Service/
│   ├── Abstractions/              # Service interfaces
│   │   ├── ICountryService.cs
│   │   ├── IIpLookupService.cs
│   │   └── IBlockCheckingService.cs
│   └── Implementations/           # Business logic
│       ├── CountryService.cs
│       ├── IpLookupService.cs
│       └── BlockCheckingService.cs
│
├── 🌐 IPCountryBlocker.API/
│   ├── Controllers/               # REST API endpoints
│   │   ├── CountriesController.cs
│   │   ├── IpLookupController.cs
│   │   ├── IpBlockController.cs
│   │   └── LogsController.cs
│   ├── Middleware/                # Custom middleware
│   │   └── ErrorHandlerMiddleware.cs
│   ├── BackgroundJobs/
│   │   └── TemporaryBlockCleanupJob.cs
│   ├── appsettings.json
│   └── Program.cs
│
└── 🧪 IPCountryBlocker.Test/
    ├── Controllers/               # Controller tests
    ├── Services/                  # Service tests
    ├── Validators/                # Validation tests
    ├── Fixtures/                  # Test fixtures
    └── Helpers/                   # Test utilities
```

### 🎨 Design Patterns

- **CQRS**: Separation of read and write operations using MediatR
- **Repository Pattern**: Abstract data access layer
- **Dependency Injection**: Built-in .NET DI container
- **Specification Pattern**: Flexible query building
- **Clean Architecture**: Clear separation of concerns

---

## 🛠️ Technology Stack

### Core Technologies
| Category | Technology | Purpose |
|----------|-----------|---------|
| **Framework** | .NET 8 | Modern, cross-platform runtime |
| **Language** | C# 12 | Latest language features |
| **Web Framework** | ASP.NET Core 8 | High-performance web API |

### Data & Caching
| Technology | Purpose |
|-----------|---------|
| **Redis** | In-memory data store |
| **StackExchange.Redis** | Redis client for .NET |
| **Newtonsoft.Json** | JSON serialization |

### Application Architecture
| Technology | Purpose |
|-----------|---------|
| **MediatR** | CQRS pattern implementation |
| **FluentValidation** | Input validation |
| **AutoMapper** | Object mapping |

### Background Processing
| Technology | Purpose |
|-----------|---------|
| **Hangfire** | Background job processing |
| **Hangfire.Redis.StackExchange** | Redis storage for Hangfire |

### External Services
| Service | Purpose | Docs |
|---------|---------|------|
| **ipapi.co** | IP geolocation (free tier) | [Docs](https://ipapi.co/) |
| **IPGeolocation.io** | IP geolocation (primary) | [Docs](https://ipgeolocation.io/) |

### Development & Testing
| Technology | Purpose |
|-----------|---------|
| **xUnit** | Unit testing framework |
| **Moq** | Mocking framework |
| **FluentAssertions** | Readable test assertions |
| **Swagger/Swashbuckle** | API documentation |

---

## 🚀 Quick Start

### Prerequisites

Ensure you have the following installed:

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ [Redis Server](https://redis.io/download) (or Docker)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- ✅ [Git](https://git-scm.com/)

### Installation

#### 1️⃣ Clone the Repository
```bash
git clone https://github.com/yourusername/IPCountryBlocker.git
cd IPCountryBlocker
```

#### 2️⃣ Start Redis Server

**Option A: Using Docker (Recommended)**
```bash
docker run -d --name redis-server -p 6379:6379 redis:latest
```

**Option B: Using Local Redis**
```bash
redis-server
```

**Verify Redis is Running:**
```bash
redis-cli ping
# Expected output: PONG
```

#### 3️⃣ Configure Application Settings

Edit `IPCountryBlocker.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "RedisConnection": "localhost:6379"
  },
  "GeoLocation": {
    "BaseUrl": "https://api.ipgeolocation.io/ipgeo",
    "ApiKey": "YOUR_API_KEY_HERE",
    "DefaultIpAddress": "8.8.8.8"
  }
}
```

**Get Your Free API Key:**
- Visit [IPGeolocation.io](https://ipgeolocation.io/) and sign up
- Free tier includes 30,000 requests/month

#### 4️⃣ Restore Dependencies
```bash
dotnet restore
```

#### 5️⃣ Build the Solution
```bash
dotnet build
```

#### 6️⃣ Run the Application
```bash
cd IPCountryBlocker.API
dotnet run
```

Or use watch mode for development:
```bash
dotnet watch run
```

#### 7️⃣ Access the Application

| Service | URL |
|---------|-----|
| 🌐 API | https://localhost:7038 |
| 📚 Swagger UI | https://localhost:7038/swagger |
| 📊 Hangfire Dashboard | https://localhost:7038/hangfire |

---

## 📡 API Documentation

### 🔐 Country Management

#### Block a Country (Permanent)
```http
POST /api/countries/block
Content-Type: application/json

{
  "code": "US",
  "name": "United States"
}
```

**Response:**
```json
{
  "succeeded": true,
  "message": "Country blocked successfully",
  "data": {
    "code": "US",
    "name": "United States",
    "isTemporary": false,
    "blockedAt": "2025-10-25T10:30:00Z"
  }
}
```

#### Block a Country (Temporary)
```http
POST /api/countries/temporal-block
Content-Type: application/json

{
  "code": "EG",
  "name": "Egypt",
  "duration": 120
}
```

**Parameters:**
- `duration`: Minutes (1-1440, i.e., 1 min to 24 hours)

**Response:**
```json
{
  "succeeded": true,
  "message": "Country temporarily blocked for 120 minutes",
  "data": {
    "code": "EG",
    "name": "Egypt",
    "isTemporary": true,
    "blockedAt": "2025-10-25T10:30:00Z",
    "expiresAt": "2025-10-25T12:30:00Z"
  }
}
```

#### Unblock a Country
```http
DELETE /api/countries/unblock/US
```

**Response:**
```json
{
  "succeeded": true,
  "message": "Country unblocked successfully"
}
```

#### Get All Blocked Countries
```http
GET /api/countries/blocked?searchItem=US&pageNumber=1&pageSize=10
```

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `searchItem` | string | - | Filter by code or name |
| `pageNumber` | int | 1 | Page number |
| `pageSize` | int | 10 | Items per page |

**Response:**
```json
{
  "succeeded": true,
  "data": [
    {
      "code": "US",
      "name": "United States",
      "isTemporary": false,
      "blockedAt": "2025-10-25T10:30:00Z"
    }
  ],
  "meta": {
    "currentPage": 1,
    "totalPages": 1,
    "pageSize": 10,
    "totalCount": 1
  }
}
```

### 🌍 IP Geolocation

#### Lookup IP Address
```http
GET /api/ip/lookup?ipAddress=8.8.8.8
```

**Response:**
```json
{
  "succeeded": true,
  "data": {
    "ip": "8.8.8.8",
    "countryCode": "US",
    "countryName": "United States",
    "city": "Mountain View",
    "isp": "Google LLC",
    "organization": "Google Public DNS",
    "latitude": 37.3860,
    "longitude": -122.0838
  }
}
```

**Features:**
- ✅ Supports IPv4 and IPv6
- ✅ Auto-detects caller IP if not specified
- ✅ Handles proxy headers (`X-Forwarded-For`, `X-Real-IP`)

### 🚫 Block Checking

#### Check if IP is Blocked
```http
GET /api/ipblock/check-block
```

**Response (IP is Blocked):**
```json
{
  "succeeded": true,
  "data": {
    "isBlocked": true,
    "ip": "154.176.153.154",
    "countryCode": "EG",
    "countryName": "Egypt",
    "reason": "Country is blocked",
    "blockedAt": "2025-10-25T10:30:00Z"
  }
}
```

**Response (IP is Allowed):**
```json
{
  "succeeded": true,
  "data": {
    "isBlocked": false,
    "ip": "8.8.8.8",
    "countryCode": "US",
    "countryName": "United States"
  }
}
```

### 📊 Logging

#### Get Blocked Attempt Logs
```http
GET /api/logs/blocked-attempts?page=1&pageSize=10&ipAddress=8.8.8.8&countryCode=US
```

**Query Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| `page` | int | Page number |
| `pageSize` | int | Items per page (max 100) |
| `ipAddress` | string | Filter by IP |
| `countryCode` | string | Filter by country |

**Response:**
```json
{
  "succeeded": true,
  "data": [
    {
      "id": "log_123456",
      "ip": "154.176.153.154",
      "countryCode": "EG",
      "countryName": "Egypt",
      "timestamp": "2025-10-25T10:30:00Z",
      "userAgent": "Mozilla/5.0...",
      "isBlocked": true
    }
  ],
  "meta": {
    "currentPage": 1,
    "totalPages": 5,
    "pageSize": 10,
    "totalCount": 50
  }
}
```

---

## ⚙️ Configuration

### Application Settings

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Hangfire": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "RedisConnection": "localhost:6379,abortConnect=false,connectTimeout=5000"
  },
  "GeoLocation": {
    "BaseUrl": "https://api.ipgeolocation.io/ipgeo",
    "ApiKey": "YOUR_API_KEY",
    "DefaultIpAddress": "8.8.8.8",
    "TimeoutSeconds": 10,
    "MaxRetries": 3
  },
  "BlockingRules": {
    "MinTemporaryBlockDuration": 1,
    "MaxTemporaryBlockDuration": 1440,
    "LogRetentionDays": 30
  },
  "Hangfire": {
    "CleanupJobCronSchedule": "*/5 * * * *"
  }
}
```

### Environment Variables

For production, use environment variables:

```bash
export ConnectionStrings__RedisConnection="your-redis-connection"
export GeoLocation__ApiKey="your-api-key"
export ASPNETCORE_ENVIRONMENT="Production"
```

### Redis Configuration

**Connection String Format:**
```
host:port,password=yourpassword,ssl=true,abortConnect=false
```

**Example for Redis Cloud:**
```
redis-12345.c123.us-east-1.ec2.cloud.redislabs.com:12345,password=yourpassword,ssl=true
```

---

## 🕒 Background Jobs

### Temporary Block Cleanup

| Property | Value |
|----------|-------|
| **Job Name** | `cleanup-temporary-blocks` |
| **Schedule** | Every 5 minutes (`*/5 * * * *`) |
| **Action** | Removes expired temporary country blocks |
| **Persistence** | Survives application restarts |

**Manual Trigger:**
```csharp
BackgroundJob.Enqueue<TemporaryBlockCleanupJob>(job => job.Execute());
```

**Monitor Jobs:**
- Dashboard: `https://localhost:7038/hangfire`
- View job history, success/failure rates, and execution times

---

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Run with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Test Structure

```
IPCountryBlocker.Test/
├── Controllers/
│   ├── CountriesControllerTests.cs       # 8 tests
│   ├── IpLookupControllerTests.cs        # 5 tests
│   └── LogsControllerTests.cs            # 4 tests
├── Services/
│   ├── CountryServiceTests.cs            # 6 tests
│   └── IpLookupServiceTests.cs           # 5 tests
└── Validators/
    └── BlockCountryValidatorTests.cs     # 4 tests
```

**Total Coverage: 32 unit tests**

---

## 🌐 Localization

### Supported Languages

```csharp
// Use query parameter
GET /api/countries/blocked?culture=ar-EG

// Or use header
GET /api/countries/blocked
Accept-Language: de-DE
```

### Add New Language

1. Create resource file: `SharedResources.{culture}.resx`
2. Add translations
3. Register in `Program.cs`:

```csharp
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "de-DE", "fr-FR", "ar-EG", "es-ES" };
    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});
```

---

## 🛡️ Error Handling

### Global Exception Middleware

All exceptions are caught and formatted consistently:

```json
{
  "succeeded": false,
  "message": "An error occurred while processing your request",
  "errors": [
    "Detailed error message"
  ],
  "statusCode": 500
}
```

### Common Status Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 400 | Bad Request (validation error) |
| 404 | Not Found |
| 409 | Conflict (duplicate entry) |
| 500 | Internal Server Error |
| 503 | Service Unavailable (Redis down) |

---

## 🚀 Deployment

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IPCountryBlocker.API.dll"]
```

### Docker Compose

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5000:80"
    environment:
      - ConnectionStrings__RedisConnection=redis:6379
      - GeoLocation__ApiKey=${GEOLOCATION_API_KEY}
    depends_on:
      - redis
  
  redis:
    image: redis:latest
    ports:
      - "6379:6379"
    volumes:
      - redis-data:/data

volumes:
  redis-data:
```

---

## 📈 Performance Considerations

### Redis Optimization
- Use connection multiplexing
- Implement connection pooling
- Set appropriate timeout values
- Use Redis pipelining for bulk operations

### API Performance
- Response caching for frequently accessed data
- Compression for large payloads
- Rate limiting to prevent abuse
- Async/await for I/O operations

---

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Code Style
- Follow Microsoft C# coding conventions
- Write unit tests for new features
- Update documentation as needed

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- [ipapi.co](https://ipapi.co/) - Free IP geolocation service
- [IPGeolocation.io](https://ipgeolocation.io/) - Comprehensive IP data
- [Hangfire](https://www.hangfire.io/) - Background job processing
- [Redis](https://redis.io/) - In-memory data structure store

---

## 📞 Support

- **Documentation**: [Wiki](https://github.com/yourusername/IPCountryBlocker/wiki)
- **Issues**: [GitHub Issues](https://github.com/yourusername/IPCountryBlocker/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/IPCountryBlocker/discussions)

---

<div align="center">

**Made with ❤️ by [Your Name](https://github.com/yourusername)**

⭐ Star this repo if you find it helpful!

</div>
