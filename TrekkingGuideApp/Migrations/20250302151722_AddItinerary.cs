using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrekkingGuideApp.Migrations
{
    /// <inheritdoc />
    public partial class AddItinerary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrekkingPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrekkingPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuideItineraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuideId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrekkingPlaceId = table.Column<int>(type: "int", nullable: false),
                    TrekkingDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuideItineraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuideItineraries_AspNetUsers_GuideId",
                        column: x => x.GuideId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuideItineraries_TrekkingPlaces_TrekkingPlaceId",
                        column: x => x.TrekkingPlaceId,
                        principalTable: "TrekkingPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuideItineraries_GuideId",
                table: "GuideItineraries",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideItineraries_TrekkingPlaceId",
                table: "GuideItineraries",
                column: "TrekkingPlaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuideItineraries");

            migrationBuilder.DropTable(
                name: "TrekkingPlaces");
        }
    }
}
