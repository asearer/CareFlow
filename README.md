# CareFlow – Clinical Documentation & Scheduling API
## Overview

CareFlow is a TherapyNotes-inspired backend platform designed for clinical scheduling, SOAP documentation, audit logging, and secure operational workflows. The system is built with **.NET 8**, follows **Clean Architecture principles**, and is **Dockerized** for development and deployment.

This project demonstrates:

* Role-based authorization for therapists and admins
* Immutable, compliance-aware clinical notes (SOAP format)
* Appointment scheduling with conflict detection
* Audit logging of sensitive actions
* Background job processing for reminders and daily summaries
* Fully Dockerized environment with PostgreSQL
* Unit and integration tests for business-critical logic

---

## Architecture

**Clean Architecture Layers:**

| Layer          | Purpose                                                                          |
| -------------- | -------------------------------------------------------------------------------- |
| Domain         | Core business entities, enums, value objects, exceptions, and rules              |
| Application    | Commands, handlers, DTOs, validators, and interfaces                             |
| Infrastructure | Persistence, repositories, authentication, background jobs, dependency injection |
| API            | Controllers, request models, middleware, and HTTP layer                          |

**Additional Features:**

* Background jobs via **Hangfire**
* JWT-based authentication
* Swagger API documentation
* Environment-variable-based configuration
* Docker Compose orchestration

---

## Core Features

### 1. Appointments

* Schedule appointments for patients
* Conflict detection to prevent overlapping sessions
* Appointment cancellation and completion
* Background job to send reminders 24 hours in advance

### 2. Clinical Notes (SOAP)

* Structured notes: Subjective, Objective, Assessment, Plan
* Notes are **immutable after signing**
* Only assigned therapists can create or sign notes

### 3. Notifications & Background Jobs

* Appointment reminders for therapists
* Daily summaries via recurring jobs
* Retry-safe, idempotent, and logging-enabled
* Background jobs managed via Hangfire

### 4. Security & Compliance

* JWT-based authentication with role-based policies
* Audit logging for all sensitive actions
* No PHI logged in system logs
* Domain-layer enforcement of business rules and immutability

---

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker](https://www.docker.com/)
* [Docker Compose](https://docs.docker.com/compose/)

---

### Running Locally

1. Clone the repository:

```bash
git clone <repo-url>
cd CareFlow
```

2. Build and start services with Docker Compose:

```bash
docker compose up --build
```

3. The services will be available at:

* API: `http://localhost:8080/swagger`
* PostgreSQL: `localhost:5432` (user: `postgres`, password: `postgres`)
* Hangfire Dashboard: `http://localhost:8080/hangfire` (Admin role required)

---

### Applying Database Migrations

After starting containers:

```bash
dotnet ef database update \
  --project src/CareFlow.Infrastructure \
  --startup-project src/CareFlow.Api
```

---

## Testing

Unit and integration tests are included:

```bash
dotnet test
```

Tests include:

* Domain invariants (appointment conflicts, note immutability)
* Application workflow tests (authorization, command handling)
* Integration tests with PostgreSQL Testcontainers

---

## Project Structure

```
CareFlow/
├── src/
│   ├── CareFlow.Domain/
│   ├── CareFlow.Application/
│   ├── CareFlow.Infrastructure/
│   └── CareFlow.Api/
├── tests/
├── Dockerfile
├── docker-compose.yml
└── README.md
```

---

## Security & Operational Considerations

* Immutable signed notes ensure audit compliance
* Audit logs track all sensitive actions
* Background jobs are idempotent and safe for retries
* JWT ensures role-based access control

---

## Technologies Used

* **.NET 8 (C#)**
* **Entity Framework Core**
* **PostgreSQL**
* **Hangfire** for background jobs
* **Docker & Docker Compose**
* **Swagger** for API documentation
* **FluentValidation** for request validation

---

## Resume/Portfolio Value

This project demonstrates:

* Designing domain-driven and compliance-aware models
* Implementing role-based authorization and audit logging
* Managing background jobs and operational workflows
* Dockerizing a full-stack backend for development and production parity
* Writing unit and integration tests for critical business rules

---

## License

This project is provided for educational and portfolio purposes.

---
