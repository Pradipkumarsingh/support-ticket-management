using Microsoft.EntityFrameworkCore;
using SupportTicketManagement.Api.Models;

namespace SupportTicketManagement.Api.Data;

public class SupportTicketContext(DbContextOptions<SupportTicketContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Role).IsRequired().HasMaxLength(50);

            entity.HasData(
                new User { Id = 1, Name = "Alice Admin", Email = "alice@example.com", Role = "Admin" },
                new User { Id = 2, Name = "Bob Support", Email = "bob@example.com", Role = "Support" },
                new User { Id = 3, Name = "Charlie User", Email = "charlie@example.com", Role = "User" },
                new User { Id = 4, Name = "Diana Support", Email = "diana@example.com", Role = "Support" },
                new User { Id = 5, Name = "Ethan Analyst", Email = "ethan@example.com", Role = "User" }
            );
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(2000);

            entity.HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(t => t.Comments)
                .WithOne(c => c.Ticket)
                .HasForeignKey(c => c.TicketId);

            entity.HasData(
                new Ticket
                {
                    Id = 1,
                    Title = "Cannot log in",
                    Description = "User reports being unable to log in with correct credentials.",
                    Priority = TicketPriority.High,
                    Status = TicketStatus.Open,
                    AssignedToUserId = 2,
                    CreatedByUserId = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Ticket
                {
                    Id = 2,
                    Title = "Feature request: Dark mode",
                    Description = "User requested a dark mode for the dashboard.",
                    Priority = TicketPriority.Medium,
                    Status = TicketStatus.InProgress,
                    AssignedToUserId = 2,
                    CreatedByUserId = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(c => c.Message).IsRequired().HasMaxLength(2000);

            entity.HasOne(c => c.CreatedByUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new Comment
                {
                    Id = 1,
                    TicketId = 1,
                    Message = "We are investigating this issue.",
                    CreatedByUserId = 2,
                    CreatedAt = DateTime.UtcNow
                }
            );
        });
    }
}

