import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TicketService, UpdateTicketRequest } from '../services/ticket.service';
import { Ticket, Comment, TicketPriority, TicketStatus, User } from '../models/ticket.models';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-ticket-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-detail.component.html',
  styleUrl: './ticket-detail.component.scss'
})
export class TicketDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly ticketService = inject(TicketService);
  private readonly userService = inject(UserService);

  ticket = signal<Ticket | null>(null);
  comments = signal<Comment[]>([]);
  users = signal<User[]>([]);

  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  editTitle = '';
  editDescription = '';
  editPriority: TicketPriority = 'Medium';
  editAssignedToUserId: number | null = null;

  newComment = '';
  commentError = signal<string | null>(null);
  commentLoading = signal<boolean>(false);
  createdByUserId = 3; // seeded "Charlie User"

  readonly availableStatusTransitions = computed<TicketStatus[]>(() => {
    const current = this.ticket()?.status;
    if (!current) {
      return [];
    }

    switch (current) {
      case 'Open':
        return ['InProgress', 'Cancelled'];
      case 'InProgress':
        return ['Resolved', 'Cancelled'];
      case 'Resolved':
        return ['Closed'];
      default:
        return [];
    }
  });

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.error.set('Invalid ticket id.');
      return;
    }

    this.loading.set(true);

    this.userService.getUsers().subscribe({
      next: (users) => this.users.set(users),
      error: () => this.error.set('Failed to load users.')
    });

    this.ticketService.getTicket(id).subscribe({
      next: (ticketWithComments) => {
        const { comments, ...ticket } = ticketWithComments;
        this.ticket.set(ticket);
        this.comments.set(comments);

        this.editTitle = ticket.title;
        this.editDescription = ticket.description;
        this.editPriority = ticket.priority;
        this.editAssignedToUserId = ticket.assignedToUserId ?? null;

        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load ticket.');
        this.loading.set(false);
      }
    });
  }

  saveTicket(): void {
    const current = this.ticket();
    if (!current) {
      return;
    }

    if (!this.editTitle.trim() || !this.editDescription.trim()) {
      this.error.set('Title and Description are required.');
      return;
    }

    const payload: UpdateTicketRequest = {
      title: this.editTitle.trim(),
      description: this.editDescription.trim(),
      priority: this.editPriority,
      assignedToUserId: this.editAssignedToUserId || undefined
    };

    this.loading.set(true);

    this.ticketService.updateTicket(current.id, payload).subscribe({
      next: () => {
        this.ticket.set({
          ...current,
          title: payload.title,
          description: payload.description,
          priority: payload.priority,
          assignedToUserId: payload.assignedToUserId ?? null
        });
        this.loading.set(false);
        this.error.set(null);
      },
      error: (err) => {
        const message = err?.error?.error || 'Failed to update ticket.';
        this.error.set(message);
        this.loading.set(false);
      }
    });
  }

  changeStatus(newStatus: TicketStatus): void {
    const current = this.ticket();
    if (!current) {
      return;
    }

    this.loading.set(true);

    this.ticketService.changeStatus(current.id, newStatus).subscribe({
      next: (response) => {
        this.ticket.set({ ...current, status: response.status });
        this.loading.set(false);
        this.error.set(null);
      },
      error: (err) => {
        const message = err?.error?.error || 'Failed to change status.';
        this.error.set(message);
        this.loading.set(false);
      }
    });
  }

  addComment(): void {
    const current = this.ticket();
    if (!current) {
      return;
    }

    this.commentError.set(null);

    if (!this.newComment.trim()) {
      this.commentError.set('Comment message is required.');
      return;
    }

    this.commentLoading.set(true);

    this.ticketService
      .addComment(current.id, {
        message: this.newComment.trim(),
        createdByUserId: this.createdByUserId
      })
      .subscribe({
        next: () => {
          // Reload ticket to get full, up-to-date comment list
          this.ticketService.getTicket(current.id).subscribe({
            next: (ticketWithComments) => {
              const { comments, ...ticket } = ticketWithComments;
              this.ticket.set(ticket);
              this.comments.set(comments);
              this.newComment = '';
              this.commentLoading.set(false);
            },
            error: () => {
              this.commentError.set('Comment was added but failed to refresh comments.');
              this.commentLoading.set(false);
            }
          });
        },
        error: (err) => {
          const message = err?.error?.error || 'Failed to add comment.';
          this.commentError.set(message);
          this.commentLoading.set(false);
        }
      });
  }
}

