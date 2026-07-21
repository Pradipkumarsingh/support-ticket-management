import { Routes } from '@angular/router';
import { TicketListComponent } from './tickets/ticket-list.component';
import { TicketCreateComponent } from './tickets/ticket-create.component';
import { TicketDetailComponent } from './tickets/ticket-detail.component';

export const routes: Routes = [
  { path: '', component: TicketListComponent },
  { path: 'tickets/new', component: TicketCreateComponent },
  { path: 'tickets/:id', component: TicketDetailComponent },
  { path: '**', redirectTo: '' }
];
