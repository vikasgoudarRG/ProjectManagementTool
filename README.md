# ProjectManagementTool

# Project Management Tool

A scalable and modular **Project Management System API**, built using **.NET Core Web API**, **Entity Framework Core**, and **Clean Architecture** principles. This backend system models complex entity relationships like Projects, Teams, Tasks, Developers, and Change Logs with advanced features like audit logging, role-based control, and extensibility.

---

## Tech Stack

| Layer             | Technologies Used                         |
|------------------ |-------------------------------------------|
| **Framework**     | ASP.NET Core Web API                      |
| **ORM**           | Entity Framework Core                     |
| **Database**      | SQL Server                                |
| **Architecture**  | Clean Architecture                        |
| **Tools**         | Swagger, FluentValidation                 |

---

## Clean Architecture Structure
 Domain // Entity Models, Enums, Interfaces
├── Application // DTOs, Interfaces, Services
├── Infrastructure // EF Core DbContext, Repositories
├── API (Presentation) // Controllers, Swagger setup

Each layer is decoupled and communicates via interfaces using Dependency Injection.
