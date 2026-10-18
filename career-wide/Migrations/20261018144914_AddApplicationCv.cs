using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace career_wide.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationCv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvFileName",
                table: "JobApplications",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvStorageName",
                table: "JobApplications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvFileName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CvStorageName",
                table: "JobApplications");
        }
    }
}
