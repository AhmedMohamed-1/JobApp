using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobAndApplicationFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CvLink",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CvLink",
                table: "JobApplications");
        }
    }
}
