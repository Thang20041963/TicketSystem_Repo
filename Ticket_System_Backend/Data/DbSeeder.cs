using Ticket_System_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Ticket_System_Backend.Data
{
    public static class DbSeeder
    {
        public static void SeedData(TicketSystemContext context)
        {
            context.Database.Migrate();

            if (context.Users.Count()>0)
            {
                return;
            }

            // ===================== USERS =====================
            var users = new List<User>
            {
                new User
                {
                    UserName    = "admin",
                    PasswordHash = HashPassword("Admin@123"),
                    Email       = "admin@ticketsystem.com",
                    FullName    = "System Administrator",
                    Role        = Role.ADMIN,
                    CreatedAt   = DateTime.UtcNow
                },
                new User
                {
                    UserName    = "support1",
                    PasswordHash = HashPassword("Support@123"),
                    Email       = "support1@ticketsystem.com",
                    FullName    = "Support Agent 1",
                    Role        = Role.SUPPORTER,
                    CreatedAt   = DateTime.UtcNow
                },
                new User
                {
                    UserName    = "support2",
                    PasswordHash = HashPassword("Support@123"),
                    Email       = "support2@ticketsystem.com",
                    FullName    = "Support Agent 2",
                    Role        = Role.SUPPORTER,
                    CreatedAt   = DateTime.UtcNow
                },
                new User
                {
                    UserName    = "employee1",
                    PasswordHash = HashPassword("Employee@123"),
                    Email       = "employee1@company.com",
                    FullName    = "John Doe",
                    Role        = Role.EMPLOYEE,
                    CreatedAt   = DateTime.UtcNow
                },
                new User
                {
                    UserName    = "employee2",
                    PasswordHash = HashPassword("Employee@123"),
                    Email       = "employee2@company.com",
                    FullName    = "Jane Smith",
                    Role        = Role.EMPLOYEE,
                    CreatedAt   = DateTime.UtcNow
                },
                new User
                {
                    UserName    = "employee3",
                    PasswordHash = HashPassword("Employee@123"),
                    Email       = "employee3@company.com",
                    FullName    = "Mike Johnson",
                    Role        = Role.EMPLOYEE,
                    CreatedAt   = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Lấy lại Id sau khi SaveChanges để gán FK
            var admin    = users[0]; // Id = 1
            var support1 = users[1]; // Id = 2
            var support2 = users[2]; // Id = 3
            var emp1     = users[3]; // Id = 4
            var emp2     = users[4]; // Id = 5
            var emp3     = users[5]; // Id = 6

            // ===================== TICKETS =====================
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    Title       = "Cannot access company email",
                    Description = "Unable to login to company email. Getting 'Authentication Failed' error.",
                    Priority    = Priority.HIGH,
                    Status      = TicketStatus.OPEN,
                    CreatorId   = emp1.Id,
                    AssigneeId  = null,
                    CreatedAt   = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt   = DateTime.UtcNow.AddDays(-5)
                },
                new Ticket
                {
                    Title       = "Printer not working on 3rd floor",
                    Description = "HP printer showing 'Paper Jam' error but no paper is stuck.",
                    Priority    = Priority.MEDIUM,
                    Status      = TicketStatus.IN_PROGRESS,
                    CreatorId   = emp2.Id,
                    AssigneeId  = support1.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt   = DateTime.UtcNow.AddDays(-1)
                },
                new Ticket
                {
                    Title       = "Request new laptop",
                    Description = "Current laptop is 5 years old and running very slow.",
                    Priority    = Priority.LOW,
                    Status      = TicketStatus.OPEN,
                    CreatorId   = emp3.Id,
                    AssigneeId  = null,
                    CreatedAt   = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt   = DateTime.UtcNow.AddDays(-2)
                },
                new Ticket
                {
                    Title       = "VPN connection issues",
                    Description = "Cannot connect to company VPN from home. Keeps disconnecting.",
                    Priority    = Priority.URGENT,
                    Status      = TicketStatus.IN_PROGRESS,
                    CreatorId   = emp1.Id,
                    AssigneeId  = support2.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt   = DateTime.UtcNow.AddHours(-2)
                },
                new Ticket
                {
                    Title       = "Software installation - Adobe Photoshop",
                    Description = "Need Adobe Photoshop CC installed on workstation for design work.",
                    Priority    = Priority.MEDIUM,
                    Status      = TicketStatus.RESOLVED,
                    CreatorId   = emp2.Id,
                    AssigneeId  = support1.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt   = DateTime.UtcNow.AddDays(-6)
                },
                new Ticket
                {
                    Title       = "Account locked after failed password attempts",
                    Description = "Account got locked after forgetting password and trying multiple times.",
                    Priority    = Priority.HIGH,
                    Status      = TicketStatus.CLOSED,
                    CreatorId   = emp3.Id,
                    AssigneeId  = support1.Id,
                    CreatedAt   = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt   = DateTime.UtcNow.AddDays(-9)
                },
                new Ticket
                {
                    Title       = "Slow internet connection",
                    Description = "Internet speed is very slow in my department. Takes forever to load websites.",
                    Priority    = Priority.MEDIUM,
                    Status      = TicketStatus.OPEN,
                    CreatorId   = emp1.Id,
                    AssigneeId  = null,
                    CreatedAt   = DateTime.UtcNow.AddHours(-5),
                    UpdatedAt   = DateTime.UtcNow.AddHours(-5)
                }
            };

            context.Tickets.AddRange(tickets);
            context.SaveChanges();

            var t1 = tickets[0]; // Cannot access email
            var t2 = tickets[1]; // Printer
            var t3 = tickets[2]; // Laptop
            var t4 = tickets[3]; // VPN
            var t5 = tickets[4]; // Photoshop
            var t6 = tickets[5]; // Account locked
            var t7 = tickets[6]; // Slow internet

            // ===================== COMMENTS =====================
            var comments = new List<Comment>
            {
                new Comment
                {
                    TicketId  = t2.Id,
                    UserId    = support1.Id,
                    Content   = "I've checked the printer. Seems like a sensor issue. Will replace the part tomorrow.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Comment
                {
                    TicketId  = t2.Id,
                    UserId    = emp2.Id,
                    Content   = "Thank you! Please let me know when it's fixed.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(1)
                },
                new Comment
                {
                    TicketId  = t4.Id,
                    UserId    = support2.Id,
                    Content   = "Please try reinstalling the VPN client from the company portal.",
                    CreatedAt = DateTime.UtcNow.AddHours(-3)
                },
                new Comment
                {
                    TicketId  = t4.Id,
                    UserId    = emp1.Id,
                    Content   = "Reinstalled but still having the same issue.",
                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new Comment
                {
                    TicketId  = t5.Id,
                    UserId    = support1.Id,
                    Content   = "Adobe Photoshop CC has been installed. Please restart your computer.",
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Comment
                {
                    TicketId  = t6.Id,
                    UserId    = support1.Id,
                    Content   = "Your account has been unlocked. Use the password reset link sent to your email.",
                    CreatedAt = DateTime.UtcNow.AddDays(-9).AddHours(2)
                }
            };

            context.Comments.AddRange(comments);
            context.SaveChanges();

            // ===================== STATUS HISTORIES =====================
            var statusHistories = new List<StatusHistory>
            {
                new StatusHistory
                {
                    TicketId  = t2.Id,
                    UserId    = support1.Id,
                    OldStatus = TicketStatus.OPEN,
                    NewStatus = TicketStatus.IN_PROGRESS,
                    ChangedAt = DateTime.UtcNow.AddDays(-2)
                },
                new StatusHistory
                {
                    TicketId  = t4.Id,
                    UserId    = support2.Id,
                    OldStatus = TicketStatus.OPEN,
                    NewStatus = TicketStatus.IN_PROGRESS,
                    ChangedAt = DateTime.UtcNow.AddHours(-4)
                },
                new StatusHistory
                {
                    TicketId  = t5.Id,
                    UserId    = support1.Id,
                    OldStatus = TicketStatus.OPEN,
                    NewStatus = TicketStatus.IN_PROGRESS,
                    ChangedAt = DateTime.UtcNow.AddDays(-6).AddHours(-2)
                },
                new StatusHistory
                {
                    TicketId  = t5.Id,
                    UserId    = support1.Id,
                    OldStatus = TicketStatus.IN_PROGRESS,
                    NewStatus = TicketStatus.RESOLVED,
                    ChangedAt = DateTime.UtcNow.AddDays(-6)
                },
                new StatusHistory
                {
                    TicketId  = t6.Id,
                    UserId    = support1.Id,
                    OldStatus = TicketStatus.OPEN,
                    NewStatus = TicketStatus.IN_PROGRESS,
                    ChangedAt = DateTime.UtcNow.AddDays(-9).AddHours(-3)
                },
                new StatusHistory
                {
                    TicketId  = t6.Id,
                    UserId    = support1.Id,
                    OldStatus = TicketStatus.IN_PROGRESS,
                    NewStatus = TicketStatus.RESOLVED,
                    ChangedAt = DateTime.UtcNow.AddDays(-9)
                },
                new StatusHistory
                {
                    TicketId  = t6.Id,
                    UserId    = emp3.Id,
                    OldStatus = TicketStatus.RESOLVED,
                    NewStatus = TicketStatus.CLOSED,
                    ChangedAt = DateTime.UtcNow.AddDays(-9).AddHours(1)
                }
            };

            context.StatusHistories.AddRange(statusHistories);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}