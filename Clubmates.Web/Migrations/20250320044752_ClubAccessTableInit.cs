using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clubmates.Web.Migrations
{
    /// <inheritdoc />
    public partial class ClubAccessTableInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubAccesses",
                columns: table => new
                {
                    ClubAccessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    ClubmatesUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClubAccessRole = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAccesses", x => x.ClubAccessId);
                    table.ForeignKey(
                        name: "FK_ClubAccesses_AspNetUsers_ClubmatesUserId",
                        column: x => x.ClubmatesUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClubAccesses_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubAccesses_ClubId",
                table: "ClubAccesses",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAccesses_ClubmatesUserId",
                table: "ClubAccesses",
                column: "ClubmatesUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubAccesses");
        }
    }
}
