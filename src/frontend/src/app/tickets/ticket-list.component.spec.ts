import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { TicketListComponent } from './ticket-list.component';

describe('TicketListComponent', () => {
  let fixture: ComponentFixture<TicketListComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketListComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TicketListComponent);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('loads tickets on init (list → detail entry point)', () => {
    fixture.detectChanges();

    const req = httpMock.expectOne((r) => r.url.includes('/api/tickets'));
    expect(req.request.method).toBe('GET');
    req.flush([
      {
        id: 1,
        title: 'Sample',
        description: 'Desc',
        priority: 'Medium',
        status: 'Open',
        assignedTo: 'Bob',
        createdBy: 'Charlie',
        createdAt: '2026-01-01T00:00:00Z',
        updatedAt: '2026-01-01T00:00:00Z'
      }
    ]);

    fixture.detectChanges();
    expect(fixture.componentInstance.tickets()?.length).toBe(1);
    expect(fixture.componentInstance.loading()).toBe(false);
  });
});
