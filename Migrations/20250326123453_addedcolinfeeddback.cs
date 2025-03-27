using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedcolinfeeddback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmittedTimestamp",
                table: "Feedbacks",
                newName: "SubmittedTime");

            migrationBuilder.AddColumn<DateOnly>(
                name: "SubmittedDate",
                table: "Feedbacks",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "SubmittedTime",
                table: "Feedbacks",
                newName: "SubmittedTimestamp");
        }
    }
}
