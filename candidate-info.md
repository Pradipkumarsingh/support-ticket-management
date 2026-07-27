# Candidate Information

Name: Pradip Kumar Singh  
Role: Software Engineer  
Primary Technology Stack: .NET, Angular

Primary AI Tool Used: Cursor  
Project Option Selected: Option 1 – Backend-Heavy Support Ticket Management System

Assessment Start Date: 2026-03-20  
Submission Date: 2026-03-27

## Project Summary

Built a small full-stack support ticket management system with a .NET 8 Web API backend, Angular 19 frontend, and SQL Server persistence. The app supports creating, listing, viewing, updating, commenting on, and changing the status of tickets with an enforced backend status state machine.

## Tools Used

- .NET 8 SDK
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- Angular 19
- Cursor as the primary AI assistant

## Setup Summary

- Backend runs from `src/backend/SupportTicketManagement.Api` with a SQL Server database configured via `appsettings.json`.
- Frontend runs from `src/frontend` using Angular 19 and calls the backend REST API (`https://localhost:7093` per `src/app/config.ts`).
- Database schema and seed scripts are under `database/`, with setup instructions in `database/setup-notes.md`.
- Backend integration tests: `dotnet test tests/backend/SupportTicketManagement.Tests/SupportTicketManagement.Tests.csproj`.
