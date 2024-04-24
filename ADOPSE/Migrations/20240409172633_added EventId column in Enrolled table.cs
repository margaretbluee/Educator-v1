using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADOPSE.Migrations
{
    /// <inheritdoc />
    public partial class addedEventIdcolumninEnrolledtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "Enrolled",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Enrolled");
        }
    }
}
