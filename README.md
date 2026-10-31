# 🏥 PHCIS Clinic Booking System

A simplified **Primary Health Care Information System (PHCIS) Clinic Booking System** built with **.NET 9**, **Clean Architecture (CQRS + MediatR)**, **ASP.NET Core Web API**, and **Blazor WebAssembly (MudBlazor)**.

---

## 1. Requirements Analysis

### Key Stakeholders
- **Patients**: Register/login, browse active PHCIS clinics, view real-time time slot availability, book appointments, receive `PHCIS-XXXXXXXX` confirmation codes, and cancel or reschedule bookings.
- **Clinic Staff (Nurses / Clerks)**: Generate daily clinic time slots, view clinic-wide appointment schedules, and manage booking capacity.
- **System Administrators**: Onboard and configure clinics (operating hours, slot duration), manage role-based access (`Patient`, `ClinicStaff`, `Admin`), and monitor system logs.

### Functional Requirements
1. **Patient Management & Authentication**: Registration and JWT-based authentication linked to a `Patient` domain profile.
2. **Clinic Discovery**: Query active clinics with operating hours and slot durations (`GET /api/clinics`).
3. **Time Slot Availability & Generation**: Filter available slots by clinic and date (`GET /api/timeslots?clinicId=&date=`) and generate daily slots (`POST /api/timeslots/generate`).
4. **Booking & Double-Booking Prevention**: Reserve slot capacity (`TimeSlot.Reserve()`), verify no duplicate patient booking exists on the same slot (`HasPatientBookingOnSlotAsync`), and issue a unique confirmation code (`PHCIS-XXXXXXXX`).
5. **Cancellation & Rescheduling**: Cancel (`PUT /api/appointments/{id}/cancel`) or reschedule (`PUT /api/appointments/{id}/reschedule`) appointments while automatically releasing slot capacity (`TimeSlot.Release()`).

### Non-Functional Requirements
- **Security**: ASP.NET Core Identity + JWT Bearer tokens, Role-Based Access Control (RBAC), and `DeleteBehavior.Restrict` on medical records.
- **Performance & Reliability**: Async EF Core queries, composite unique indexes on `(ClinicId, StartTime)` and `ConfirmationCode`.
- **Observability**: Structured logging with **Serilog** and standardized RFC 7807 `ProblemDetails` error responses via `ExceptionHandlingMiddleware`.
- **Maintainability**: SOLID principles, Domain-Driven Design (DDD) entities, MediatR CQRS pipeline behaviours, and xUnit + Moq + FluentAssertions test suites.

---

## 2. Solution Architecture

```text
Clinic.slnx
├── src/
│   ├── Clinic.Domain/                  # Enterprise entities, enums, domain events & exceptions
│   ├── Clinic.Application/             # CQRS commands/queries (MediatR), FluentValidation, AutoMapper
│   ├── Clinic.Infrastructure/          # EF Core DbContext, SQL Server configs, Repositories, Identity, Seeder
│   ├── Clinic.API/                     # ASP.NET Core Web API, JWT Auth, Swagger/OpenAPI, Serilog, Middleware
│   └── Clinic.Web/                     # Blazor WebAssembly SPA with MudBlazor UI components
├── tests/
│   ├── Clinic.Domain.Tests/            # Domain entity & invariant unit tests (16 tests)
│   ├── Clinic.Application.Tests/       # CQRS handler & validator unit tests with Moq (11 tests)
│   └── Clinic.Infrastructure.Tests/    # EF Core InMemory repository integration tests (6 tests)
├── Dockerfile                          # Multi-stage Docker build for Clinic.API
├── docker-compose.yml                  # SQL Server 2022 + Clinic.API orchestration
└── .github/workflows/ci.yml            # GitHub Actions build & test pipeline
```

---

## 3. Getting Started

### Build & Run Tests
```powershell
dotnet build Clinic.slnx
dotnet test Clinic.slnx
```

### Run the Web API (auto-seeds 3 Cape Town PHCIS clinics & 7 days of time slots in Development)
```powershell
dotnet run --project src/Clinic.API/Clinic.API.csproj
```
- Swagger UI: `https://localhost:7001/swagger`

### Run the Blazor WASM Frontend
```powershell
dotnet run --project src/Clinic.Web/Clinic.Web.csproj
```

### Run with Docker Compose
```powershell
docker-compose up --build
```
