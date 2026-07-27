# API Contract

Base URL (local HTTPS profile): `https://localhost:7093`

## Users

### `GET /api/users`

Returns seeded users for assignee/creator dropdowns.

**Response:** `200` — array of `{ id, name, email, role }`

## Tickets

### `GET /api/tickets?search={term}&status={TicketStatus}`

List tickets; optional keyword search (title/description) and status filter.

**Response:** `200` — array of ticket summaries.

### `GET /api/tickets/{id}`

Ticket detail including `comments[]`.

**Response:** `200` | `404`

### `POST /api/tickets`

**Body:**

```json
{
  "title": "string",
  "description": "string",
  "priority": "Low | Medium | High",
  "createdByUserId": 1,
  "assignedToUserId": 2
}
```

**Response:** `201` `{ "id": number }` | `400` validation

### `PUT /api/tickets/{id}`

Update title, description, priority, assignee.

**Response:** `204` | `400` | `404`

### `POST /api/tickets/{id}/status`

**Body:** `{ "newStatus": "Open | InProgress | Resolved | Closed | Cancelled" }`

**Response:** `200` `{ id, status }` | `400` invalid transition | `404`

### `POST /api/tickets/{id}/comments`

**Body:** `{ "message": "string", "createdByUserId": number }`

**Response:** `201` `{ "id": number }` | `400` | `404`

## Error format

Validation and business rule failures typically return `400` with `{ "error": "message" }`.
