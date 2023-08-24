using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class changeUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Uploads");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoredFileName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "StoredFileName",
                table: "Uploads");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
