import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { APP_CONFIG } from '../config';
import { Ticket, TicketPriority, TicketStatus, Comment } from '../models/ticket.models';

export interface CreateTicketRequest {
  title: string;
  description: string;
  priority: TicketPriority;
  createdByUserId: number;
  assignedToUserId?: number | null;
}

export interface UpdateTicketRequest {
  title: string;
  description: string;
  priority: TicketPriority;
  assignedToUserId?: number | null;
}

export interface ChangeStatusRequest {
  newStatus: TicketStatus;
}

export interface AddCommentRequest {
  message: string;
  createdByUserId: number;
}

@Injectable({ providedIn: 'root' })
export class TicketService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${APP_CONFIG.apiBaseUrl}/api/tickets`;

  getTickets(search?: string, status?: TicketStatus | ''): Observable<Ticket[]> {
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<Ticket[]>(this.baseUrl, { params });
  }

  getTicket(id: number): Observable<Ticket & { comments: Comment[] }> {
    return this.http.get<Ticket & { comments: Comment[] }>(`${this.baseUrl}/${id}`);
  }

  createTicket(payload: CreateTicketRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.baseUrl, payload);
  }

  updateTicket(id: number, payload: UpdateTicketRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  changeStatus(id: number, newStatus: TicketStatus): Observable<{ id: number; status: TicketStatus }> {
    const body: ChangeStatusRequest = { newStatus };
    return this.http.post<{ id: number; status: TicketStatus }>(`${this.baseUrl}/${id}/status`, body);
  }

  addComment(id: number, payload: AddCommentRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.baseUrl}/${id}/comments`, payload);
  }
}

