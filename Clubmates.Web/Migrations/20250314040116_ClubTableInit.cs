using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clubmates.Web.Migrations
{
    /// <inheritdoc />
    public partial class ClubTableInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClubDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClubCategory = table.Column<int>(type: "int", nullable: false),
                    ClubType = table.Column<int>(type: "int", nullable: false),
                    ClubRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClubManagerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClubContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClubEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                    table.ForeignKey(
                        name: "FK_Clubs_AspNetUsers_ClubManagerId",
                        column: x => x.ClubManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ClubManagerId",
                table: "Clubs",
                column: "ClubManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}
