using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookwormAPI.Migrations;

/// <inheritdoc />
public partial class _20261119114328_AddUserIdToBook : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Books_Users_UserId",
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

        migrationBuilder.AddForeignKey(
            name: "FK_Books_Users_UserId",
            table: "Books",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Books_Users_UserId",
            table: "Books");

        migrationBuilder.AlterColumn<int>(
            name: "UserId",
            table: "Books",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");

        migrationBuilder.AddForeignKey(
            name: "FK_Books_Users_UserId",
            table: "Books",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id");
    }
}
