using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp_dotnet_initiative.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAndCompletedAtToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TaskItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TaskItems");
        }
    }
}
