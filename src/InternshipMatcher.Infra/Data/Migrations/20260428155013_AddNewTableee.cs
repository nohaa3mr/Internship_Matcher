using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipMatcher.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTableee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentApplications",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentProfileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternshipID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationFormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentApplications", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentApplications");
        }
    }
}
