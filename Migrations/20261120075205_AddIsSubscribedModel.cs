using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookwormAPI.Migrations;

/// <inheritdoc />
public partial class _20261120075205_AddIsSubscribedModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsSubscribed",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsSubscribed",
            table: "Users");
    }
}
