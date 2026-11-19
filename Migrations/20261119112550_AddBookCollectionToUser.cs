using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookwormAPI.Migrations;

/// <inheritdoc />
public partial class _20261119112550_AddBookCollectionToUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "Books",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.CreateIndex(
            name: "IX_Books_UserId",
            table: "Books",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Books_Users_UserId",
            table: "Books",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Books_Users_UserId",
            table: "Books");

        migrationBuilder.DropIndex(
            name: "IX_Books_UserId",
            table: "Books");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "Books",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);
    }
}
