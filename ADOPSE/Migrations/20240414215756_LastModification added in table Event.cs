using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADOPSE.Migrations
{
    /// <inheritdoc />
    public partial class LastModificationaddedintableEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoogleCalendarID",
                table: "Event",
                newName: "GoogleCalendarEventID");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModification",
                table: "Event",
                type: "datetime(6)",
                nullable: false); ;
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModification",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "GoogleCalendarEventID",
                table: "Event",
                newName: "GoogleCalendarID");
        }
    }
}
