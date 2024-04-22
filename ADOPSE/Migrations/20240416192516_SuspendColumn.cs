using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADOPSE.Migrations
{
    /// <inheritdoc />
    public partial class SuspendColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Suspend",
                table: "USERS",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suspend",
                table: "USERS");
        }
    }
}
