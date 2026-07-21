import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TicketService, CreateTicketRequest } from '../services/ticket.service';
import { User, TicketPriority } from '../models/ticket.models';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-ticket-create',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-create.component.html',
  styleUrl: './ticket-create.component.scss'
})
export class TicketCreateComponent implements OnInit {
  private readonly ticketService = inject(TicketService);
  private readonly userService = inject(UserService);
  private readonly router = inject(Router);

  users = signal<User[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  title = '';
  description = '';
  priority: TicketPriority = 'Medium';
  assignedToUserId: number | null = null;
  createdByUserId = 3; // seeded "Charlie User"

  ngOnInit(): void {
    this.userService.getUsers().subscribe({
      next: (users) => this.users.set(users),
      error: () => this.error.set('Failed to load users.')
    });
  }

  submit(): void {
    this.error.set(null);

    if (!this.title.trim() || !this.description.trim()) {
      this.error.set('Title and Description are required.');
      return;
    }

    const payload: CreateTicketRequest = {
      title: this.title.trim(),
      description: this.description.trim(),
      priority: this.priority,
      createdByUserId: this.createdByUserId,
      assignedToUserId: this.assignedToUserId || undefined
    };

    this.loading.set(true);

    this.ticketService.createTicket(payload).subscribe({
      next: (response) => {
        this.loading.set(false);
        this.router.navigate(['/tickets', response.id]);
      },
      error: (err) => {
        const message = err?.error?.error || 'Failed to create ticket.';
        this.error.set(message);
        this.loading.set(false);
      }
    });
  }
}

