using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VelocityBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItems_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@velocityboard.com", "Admin", "User", "$2y$10$ievgpt4SzPDI30dKyajspuWyeW2uQ6KvCsQZIa3koo1OT6g1Zm006", "admin" },
                    { 2, "john.doe@example.com", "John", "Doe", "$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcflpP3OvI0Cz", "john.doe" },
                    { 3, "jane.smith@example.com", "Jane", "Smith", "$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcflpP3OvI0Cz", "jane.smith" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedByUserId", "CreatedDate", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5010), null, "Website Redesign" },
                    { 2, 2, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5023), null, "Mobile App Development" },
                    { 3, 3, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5025), null, "Marketing Campaign" }
                });

            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "AssignedToUserId", "CreatedDate", "Description", "DueDate", "Priority", "ProjectId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5053), null, new DateTime(2025, 12, 22, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5045), 2, 1, 0, "Design Homepage" },
                    { 2, 1, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5056), null, new DateTime(2025, 12, 20, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5054), 2, 1, 1, "Setup Database" },
                    { 3, 3, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5058), null, new DateTime(2025, 12, 27, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5057), 1, 2, 0, "Develop Login API" },
                    { 4, 3, new DateTime(2025, 12, 17, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5061), null, new DateTime(2025, 12, 24, 20, 37, 42, 785, DateTimeKind.Local).AddTicks(5059), 0, 3, 0, "Create Ad Copy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedByUserId",
                table: "Projects",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedToUserId",
                table: "TaskItems",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_ProjectId",
                table: "TaskItems",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
