# UI Flow

## Routes

| Path | Screen |
|------|--------|
| `/` | Ticket list (search, status filter, links to detail) |
| `/tickets/new` | Create ticket |
| `/tickets/:id` | Ticket detail (edit, status, comments) |

## Create → list → detail

1. User opens **Create Ticket**, fills title, description, priority, creator and assignee (from seeded users).
2. On success, navigates to list or detail (implementation navigates per component logic after create).
3. List shows all tickets; user can filter and open a row.
4. Detail shows metadata, allows edit/save, pick allowed status transitions, and add comments.

## States

- **Loading** — Shown while HTTP requests are in flight on list and detail.
- **Empty** — No tickets or no comments message.
- **Error** — User-visible message when API fails (network or 4xx/5xx).

## Status actions

Detail view offers status changes that match backend rules; invalid attempts show API error text.

See `src/frontend/src/app/app.routes.ts` and components under `src/app/tickets/`.
