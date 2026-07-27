# Data Model

## User

| Field | Type | Notes |
|-------|------|--------|
| Id | int | PK |
| Name | string(100) | Required |
| Email | string(200) | Required |
| Role | string(50) | e.g. Admin, Support, User |

Seeded only (no registration UI).

## Ticket

| Field | Type | Notes |
|-------|------|--------|
| Id | int | PK |
| Title | string(200) | Required |
| Description | string(2000) | Required |
| Priority | enum | Low, Medium, High |
| Status | enum | Open, InProgress, Resolved, Closed, Cancelled |
| AssignedToUserId | int? | FK → User |
| CreatedByUserId | int | FK → User |
| CreatedAt | DateTime (UTC) | |
| UpdatedAt | DateTime (UTC) | |

## Comment

| Field | Type | Notes |
|-------|------|--------|
| Id | int | PK |
| TicketId | int | FK → Ticket |
| Message | string(2000) | Required |
| CreatedByUserId | int | FK → User |
| CreatedAt | DateTime (UTC) | |

## Relationships

- User 1—* Tickets (created, assigned)
- Ticket 1—* Comments
- User 1—* Comments

EF Core configuration and seed data: `src/backend/SupportTicketManagement.Api/Data/SupportTicketContext.cs`.  
SQL scripts: `database/schema-or-migrations/`, `database/seed-data/`.
