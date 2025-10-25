IP Country Blocker API

A comprehensive .NET 8 Web API for managing blocked countries and validating IP addresses using third-party geolocation services.
The application uses in-memory Redis storage and provides advanced features for country blocking, IP geolocation, and attempt logging.

📋 Table of Contents

Features

Technology Stack

Project Architecture

Setup Instructions

API Endpoints

Configuration

Background Jobs

Localization

Error Handling

🚀 Features
✅ Core Features
1. Country Management

Add blocked countries (permanent)

Remove blocked countries from the blocked list

Get all blocked countries with pagination

Search/filter blocked countries by code or name

Prevent duplicate country entries

ISO 3166-1 alpha-2 country code validation (e.g., US, GB, EG)

2. Temporary Country Blocking

Block countries for a specific duration (1–1440 minutes / 24 hours)

Automatic expiration and cleanup of expired blocks

Prevent duplicate temporal blocks

Background job runs every 5 minutes (Hangfire)

3. IP Geolocation Lookup

Get country details from IP (code, name, city, ISP, org, coordinates)

IP format validation (IPv4 & IPv6)

Fallback to caller’s IP via HttpContext

Proxy header support (X-Forwarded-For, X-Real-IP)

Third-party integration (ipapi.co, IPGeolocation.io)

Built-in rate limiting and error handling

4. IP Block Checking

Check if caller’s IP is blocked

Automatic country code lookup

Compare against blocked country list

Log all check attempts automatically

5. Blocked Attempt Logging

Log IP block attempts (IP, country, timestamp, user agent, status)

Paginated log retrieval

Filter by IP or country

30-day auto-retention (configurable)

Redis-backed in-memory storage

6. Data Persistence

Redis for high-speed in-memory data storage

Thread-safe operations

TTL support for auto-expiration

Optimized key-value storage

7. Validation & Input Handling

FluentValidation for input validation

ISO 3166-1 alpha-2 validation

Duration range (1–1440)

Duplicate prevention

Localized validation messages

8. Localization

Multi-language: en-US, de-DE, fr-FR, ar-EG

Support for query parameter ?culture=ar-EG

Accept-Language header support

All response messages localized

9. API Documentation

Swagger/OpenAPI integration

Full endpoint and schema documentation

Available at /swagger

10. Background Services

Hangfire recurring job for cleanup

CRON: */5 * * * *

Dashboard at /hangfire

11. Testing

Unit tests with xUnit

Mocking with Moq

20+ test cases

FluentAssertions for readable tests

🧰 Technology Stack
Category	Technology
Core	.NET 8, C# 12, ASP.NET Core 8
Data & Caching	Redis (StackExchange.Redis), Newtonsoft.Json
CQRS	MediatR
Validation	FluentValidation
Background Jobs	Hangfire
HTTP	HttpClient
Geolocation	ipapi.co / IPGeolocation.io
Documentation	Swagger, Swashbuckle
🏗️ Project Architecture
IPCountryBlocker.Domain/
 ├── Models/           (Business entities)
 ├── Interfaces/       (Repository & service contracts)
 └── DTOs/             (Data transfer objects)

IPCountryBlocker.Application/
 ├── Features/         (Commands, Queries, Handlers)
 ├── Validators/       (FluentValidation rules)
 ├── Bases/            (Response handler, base classes)
 ├── Resources/        (Localization)
 └── DTOs/             (Request/response DTOs)

IPCountryBlocker.Infrastructure/
 ├── Repositories/     (Redis-backed repositories)
 ├── Specifications/   (Query specifications)
 └── ModuleInfrastructureDependencies.cs

IPCountryBlocker.Service/
 ├── Abstractions/     (Service interfaces)
 ├── Implementations/  (Business logic services)
 └── ModuleServiceDependencies.cs

IPCountryBlocker.API/
 ├── Controllers/      (REST endpoints)
 ├── Middleware/       (Error handling)
 └── Program.cs        (Startup)

IPCountryBlocker.Test/
 ├── Controllers/      (Controller unit tests)
 ├── Fixtures/         (Test fixtures)
 ├── Helpers/          (Test utilities)
 └── IPCountryBlocker.Test.csproj

⚙️ Setup Instructions
Prerequisites

.NET 8 SDK

Redis Server (local or Docker)

Visual Studio 2022 or VS Code

Installation
# 1. Clone the repository
git clone https://github.com/yourusername/IPCountryBlocker.git

# 2. Navigate to folder
cd IPCountryBlocker

# 3. Restore dependencies
dotnet restore

# 4. Run Redis
docker run -d -p 6379:6379 redis:latest
# OR
redis-server

# 5. Run the API
dotnet run --project IPCountryBlocker.API

# 6. Access:
# API: https://localhost:7038
# Swagger: https://localhost:7038/swagger
# Hangfire: https://localhost:7038/hangfire

📡 API Endpoints
🧱 Country Management
1. Block a Country
POST /api/countries/block
Content-Type: application/json

{
  "code": "US",
  "name": "United States"
}

2. Temporarily Block a Country
POST /api/countries/temporal-block
Content-Type: application/json

{
  "code": "EG",
  "name": "Egypt",
  "duration": 120
}

3. Unblock a Country
DELETE /api/countries/unblock/{code}

4. Get All Blocked Countries
GET /api/countries/blocked?searchItem=US&pageNumber=1&pageSize=10

🌍 IP Geolocation
5. IP Lookup
GET /api/ip/lookup?ipAddress=8.8.8.8

6. Check If IP is Blocked
GET /api/ipblock/check-block

🧾 Logging
7. Get Blocked Attempt Logs
GET /api/logs/blocked-attempts?page=1&pageSize=10

🧩 Configuration
appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "RedisConnection": "localhost:6379"
  },
  "GeoLocation": {
    "BaseUrl": "https://api.ipgeolocation.io/ipgeo",
    "ApiKey": "YOUR_API_KEY",
    "DefaultIpAddress": "154.176.153.154"
  }
}

Key Settings
Setting	Default	Description
RedisConnection	localhost:6379	Redis connection
GeoLocation.BaseUrl	https://api.ipgeolocation.io/ipgeo	API endpoint
GeoLocation.ApiKey	—	API key
Temporary Block Duration	1–1440	Valid duration (minutes)
Block Cleanup Job	*/5 * * * *	CRON for cleanup
🕒 Background Jobs
Temporary Block Cleanup
Property	Value
Job Name	cleanup-temporary-blocks
Schedule	Every 5 minutes (*/5 * * * *)
Action	Removes expired temporary country blocks
