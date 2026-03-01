using Microsoft.EntityFrameworkCore;

namespace Ticket_System_Backend.Models
{
    public class TicketSystemContext : DbContext
    {
        public TicketSystemContext()
        {
        }

        public TicketSystemContext(DbContextOptions<TicketSystemContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback for EF design-time tools (Add-Migration, Update-Database)
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseOracle(config.GetConnectionString("DefaultConnection"));
            }
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<StatusHistory> StatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TICKET_USER");

            // ===================== USERS TABLE =====================
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.FullName)
                    .HasMaxLength(100);

                entity.Property(u => u.Role)
                    .IsRequired();

                // Indexes cho USERS
                entity.HasIndex(u => u.UserName)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Username");

                entity.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Email");

                entity.HasIndex(u => u.Role)
                    .HasDatabaseName("IX_Users_Role");
            });

            // ===================== TICKETS TABLE =====================
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("TICKETS");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(t => t.Description)
                    .HasMaxLength(2000);

                entity.Property(t => t.Priority)
                    .IsRequired();

                entity.Property(t => t.Status)
                    .IsRequired();

                // Relationships
                entity.HasOne(t => t.Creator)
                    .WithMany(u => u.CreatedTickets)
                    .HasForeignKey(t => t.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Assignee)
                    .WithMany(u => u.AssignedTickets)
                    .HasForeignKey(t => t.AssigneeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                // Indexes cho TICKETS
                entity.HasIndex(t => t.CreatorId)
                    .HasDatabaseName("IX_Tickets_CreatorId");

                entity.HasIndex(t => t.AssigneeId)
                    .HasDatabaseName("IX_Tickets_AssigneeId");

                entity.HasIndex(t => t.Status)
                    .HasDatabaseName("IX_Tickets_Status");

                entity.HasIndex(t => t.Priority)
                    .HasDatabaseName("IX_Tickets_Priority");

                entity.HasIndex(t => t.CreatedAt)
                    .HasDatabaseName("IX_Tickets_CreatedAt");

                // Composite Index cho tìm kiếm phổ biến
                entity.HasIndex(t => new { t.Status, t.Priority })
                    .HasDatabaseName("IX_Tickets_Status_Priority");

                entity.HasIndex(t => new { t.AssigneeId, t.Status })
                    .HasDatabaseName("IX_Tickets_AssigneeId_Status");
            });

            // ===================== COMMENTS TABLE =====================
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("COMMENTS");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Content)
                    .IsRequired()
                    .HasMaxLength(1000);

                // Relationships
                entity.HasOne(c => c.Ticket)
                    .WithMany(t => t.Comments)
                    .HasForeignKey(c => c.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes cho COMMENTS
                entity.HasIndex(c => c.TicketId)
                    .HasDatabaseName("IX_Comments_TicketId");

                entity.HasIndex(c => c.UserId)
                    .HasDatabaseName("IX_Comments_UserId");

                entity.HasIndex(c => c.CreatedAt)
                    .HasDatabaseName("IX_Comments_CreatedAt");

                // Composite Index
                entity.HasIndex(c => new { c.TicketId, c.CreatedAt })
                    .HasDatabaseName("IX_Comments_TicketId_CrAt");
            });

            // ===================== STATUS_HISTORIES TABLE =====================
            modelBuilder.Entity<StatusHistory>(entity =>
            {
                entity.ToTable("STATUS_HISTORIES");
                entity.HasKey(sh => sh.Id);

                entity.Property(sh => sh.OldStatus)
                    .IsRequired();

                entity.Property(sh => sh.NewStatus)
                    .IsRequired();

                // Relationships
                entity.HasOne(sh => sh.Ticket)
                    .WithMany(t => t.StatusHistories)
                    .HasForeignKey(sh => sh.TicketId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                entity.HasOne(sh => sh.User)
                    .WithMany()
                    .HasForeignKey(sh => sh.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                // Indexes cho STATUS_HISTORIES
                entity.HasIndex(sh => sh.TicketId)
                    .HasDatabaseName("IX_StatusHistories_TicketId");

                entity.HasIndex(sh => sh.UserId)
                    .HasDatabaseName("IX_StatusHistories_ChangedById");

                entity.HasIndex(sh => sh.ChangedAt)
                    .HasDatabaseName("IX_StatusHistories_ChangedAt");

                // Composite Index
                entity.HasIndex(sh => new { sh.TicketId, sh.ChangedAt })
                    .HasDatabaseName("IX_SH_TicketId_ChangedAt");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
