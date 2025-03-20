using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clubmates.Web.Migrations
{
    /// <inheritdoc />
    public partial class ClubTableAddedLogoBannerBackground : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ClubBackground",
                table: "Clubs",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ClubBanner",
                table: "Clubs",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ClubLogo",
                table: "Clubs",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubBackground",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ClubBanner",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ClubLogo",
                table: "Clubs");
        }
    }
}
