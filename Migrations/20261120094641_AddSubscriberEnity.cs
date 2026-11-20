using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookwormAPI.Migrations;

/// <inheritdoc />
public partial class _20261120094641_AddSubscriberEnity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Subscribers",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Email = table.Column<string>(type: "text", nullable: false),
                IsPlatformUser = table.Column<bool>(type: "boolean", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                SubscribedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UnsubscribedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                UserId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subscribers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Subscribers_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Subscribers_Email",
            table: "Subscribers",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Subscribers_UserId",
            table: "Subscribers",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Subscribers");
    }
}
