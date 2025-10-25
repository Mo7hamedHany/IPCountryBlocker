# IP Country Blocker API

A comprehensive .NET 8 Web API for managing blocked countries and validating IP addresses using third-party geolocation services. The application uses in-memory Redis storage and provides advanced features for country blocking, IP geolocation, and attempt logging.

## Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Architecture](#project-architecture)
- [Setup Instructions](#setup-instructions)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Background Jobs](#background-jobs)
- [Localization](#localization)
- [Error Handling](#error-handling)

---

## Features

### ✅ Core Features

#### 1. **Country Management**
- ✓ Add blocked countries (permanent)
- ✓ Remove blocked countries from the blocked list
- ✓ Get all blocked countries with pagination
- ✓ Search/filter blocked countries by code or name
- ✓ Prevent duplicate country entries
- ✓ ISO 3166-1 alpha-2 country code validation (e.g., "US", "GB", "EG")

#### 2. **Temporary Country Blocking**
- ✓ Block countries for a specific duration (1-1440 minutes / 24 hours)
- ✓ Automatic expiration and cleanup of expired blocks
- ✓ Prevent duplicate temporal blocks
- ✓ Background job runs every 5 minutes to remove expired blocks using Hangfire

#### 3. **IP Geolocation Lookup**
- ✓ Get country details from IP addresses (country code, name, city, ISP, org, coordinates)
- ✓ IP address format validation (IPv4 & IPv6)
- ✓ Automatic fallback to caller's IP via HttpContext if IP not provided
- ✓ Support for proxy headers (X-Forwarded-For, X-Real-IP)
- ✓ Integration with third-party geolocation APIs (ipapi.co, IPGeolocation.io)
- ✓ Rate limiting and error handling

#### 4. **IP Block Checking**
- ✓ Check if caller's IP is blocked
- ✓ Automatic country code lookup
- ✓ Check against blocked country list
- ✓ Automatic logging of all check attempts

#### 5. **Blocked Attempt Logging**
- ✓ Log all IP block check attempts with details:
  - IP address
  - Country code & name
  - Timestamp
  - User agent
  - Block status
- ✓ Paginated log retrieval
- ✓ Filter by country and IP address
- ✓ 30-day automatic retention (configurable)
- ✓ Redis-backed in-memory storage

#### 6. **Data Persistence**
- ✓ Redis for fast in-memory storage
- ✓ Thread-safe operations using Redis native concurrency
- ✓ TTL (Time-To-Live) support for automatic expiration
- ✓ Efficient key-value and list storage patterns

#### 7. **Validation & Input Handling**
- ✓ FluentValidation for command/query validation
- ✓ ISO 3166-1 alpha-2 format validation
- ✓ Duration range validation (1-1440 minutes)
- ✓ Duplicate prevention across permanent and temporary blocks
- ✓ Custom validation rules with localized error messages

#### 8. **Localization Support**
- ✓ Multi-language support: English (en-US), German (de-DE), French (fr-FR), Arabic (ar-EG)
- ✓ Query string parameter support (`?culture=ar-EG`)
- ✓ Accept-Language header support
- ✓ All response messages localized

#### 9. **API Documentation**
- ✓ Swagger/OpenAPI integration
- ✓ Full endpoint documentation
- ✓ Request/response schema documentation
- ✓ Accessible at `/swagger`

#### 10. **Background Services**
- ✓ Hangfire recurring job for temporary block cleanup
- ✓ Runs every 5 minutes (configurable CRON expression)
- ✓ Automatic removal of expired blocks
- ✓ Hangfire dashboard at `/hangfire`

#### 11. **Comprehensive Testing**
- ✓ Unit tests using xUnit
- ✓ Mocking with Moq
- ✓ 20+ test cases covering all controller actions
- ✓ FluentAssertions for readable test assertions
- ✓ Integration-like workflow tests

---

## Technology Stack

### Core Framework
- **.NET 8** - Latest .NET framework
- **C# 12.0** - Modern C# language features
- **ASP.NET Core 8.0** - Web API framework

### Data & Caching
- **Redis** - In-memory data store via StackExchange.Redis
- **Newtonsoft.Json** - JSON serialization

### CQRS & Mediator Pattern
- **MediatR** - Command Query Responsibility Segregation pattern
- **FluentValidation** - Validation framework

### Background Jobs
- **Hangfire** - Recurring job scheduler
- **In-Memory Storage** - Hangfire job persistence

### HTTP & External APIs
- **HttpClient** - Third-party API integration
- **ipapi.co** / **IPGeolocation.io** - Geolocation service providers

### API Documentation
- **Swagger/OpenAPI** - API documentation
- **Swashbuckle** - Swagger integration
