using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TICKET_USER");

            migrationBuilder.CreateTable(
                name: "USERS",
                schema: "TICKET_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    Role = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "REFRESH_TOKENS",
                schema: "TICKET_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Token = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REFRESH_TOKENS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REFRESH_TOKENS_USERS_UserId",
                        column: x => x.UserId,
                        principalSchema: "TICKET_USER",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TICKETS",
                schema: "TICKET_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Title = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    Priority = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CreatorId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AssigneeId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Category = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TICKETS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TICKETS_USERS_AssigneeId",
                        column: x => x.AssigneeId,
                        principalSchema: "TICKET_USER",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TICKETS_USERS_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "TICKET_USER",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COMMENTS",
                schema: "TICKET_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Content = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TicketId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COMMENTS_TICKETS_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "TICKET_USER",
                        principalTable: "TICKETS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMMENTS_USERS_UserId",
                        column: x => x.UserId,
                        principalSchema: "TICKET_USER",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "STATUS_HISTORIES",
                schema: "TICKET_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TicketId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OldStatus = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NewStatus = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATUS_HISTORIES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_STATUS_HISTORIES_TICKETS_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "TICKET_USER",
                        principalTable: "TICKETS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_STATUS_HISTORIES_USERS_UserId",
                        column: x => x.UserId,
                        principalSchema: "TICKET_USER",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedAt",
                schema: "TICKET_USER",
                table: "COMMENTS",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                schema: "TICKET_USER",
                table: "COMMENTS",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId_CrAt",
                schema: "TICKET_USER",
                table: "COMMENTS",
                columns: new[] { "TicketId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                schema: "TICKET_USER",
                table: "COMMENTS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                schema: "TICKET_USER",
                table: "REFRESH_TOKENS",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "TICKET_USER",
                table: "REFRESH_TOKENS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SH_TicketId_ChangedAt",
                schema: "TICKET_USER",
                table: "STATUS_HISTORIES",
                columns: new[] { "TicketId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_ChangedAt",
                schema: "TICKET_USER",
                table: "STATUS_HISTORIES",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_ChangedById",
                schema: "TICKET_USER",
                table: "STATUS_HISTORIES",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_TicketId",
                schema: "TICKET_USER",
                table: "STATUS_HISTORIES",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssigneeId",
                schema: "TICKET_USER",
                table: "TICKETS",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssigneeId_Status",
                schema: "TICKET_USER",
                table: "TICKETS",
                columns: new[] { "AssigneeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedAt",
                schema: "TICKET_USER",
                table: "TICKETS",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatorId",
                schema: "TICKET_USER",
                table: "TICKETS",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Priority",
                schema: "TICKET_USER",
                table: "TICKETS",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status",
                schema: "TICKET_USER",
                table: "TICKETS",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status_Priority",
                schema: "TICKET_USER",
                table: "TICKETS",
                columns: new[] { "Status", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "TICKET_USER",
                table: "USERS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role",
                schema: "TICKET_USER",
                table: "USERS",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "TICKET_USER",
                table: "USERS",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMMENTS",
                schema: "TICKET_USER");

            migrationBuilder.DropTable(
                name: "REFRESH_TOKENS",
                schema: "TICKET_USER");

            migrationBuilder.DropTable(
                name: "STATUS_HISTORIES",
                schema: "TICKET_USER");

            migrationBuilder.DropTable(
                name: "TICKETS",
                schema: "TICKET_USER");

            migrationBuilder.DropTable(
                name: "USERS",
                schema: "TICKET_USER");
        }
    }
}
