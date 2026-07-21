import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TicketService } from '../services/ticket.service';
import { Ticket, TicketStatus } from '../models/ticket.models';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.scss'
})
export class TicketListComponent implements OnInit {
  private readonly ticketService = inject(TicketService);

  tickets = signal<Ticket[] | null>(null);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  searchTerm = '';
  statusFilter: TicketStatus | '' = '';

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.loading.set(true);
    this.error.set(null);

    this.ticketService
      .getTickets(
        this.searchTerm.trim() || undefined,
        this.statusFilter || ''
      )
      .subscribe({
        next: (tickets) => {
          this.tickets.set(tickets);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Failed to load tickets. Please try again.');
          this.loading.set(false);
        }
      });
  }

  applyFilters(): void {
    this.loadTickets();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.statusFilter = '';
    this.loadTickets();
  }
}

