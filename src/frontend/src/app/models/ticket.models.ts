export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

export type TicketPriority = 'Low' | 'Medium' | 'High';
export type TicketStatus = 'Open' | 'InProgress' | 'Resolved' | 'Closed' | 'Cancelled';

export interface Ticket {
  id: number;
  title: string;
  description: string;
  priority: TicketPriority;
  status: TicketStatus;
  assignedToUserId?: number | null;
  assignedTo?: string | null;
  createdByUserId?: number;
  createdBy?: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface Comment {
  id: number;
  ticketId: number;
  message: string;
  createdByUserId: number;
  createdBy?: string | null;
  createdAt: string;
}

