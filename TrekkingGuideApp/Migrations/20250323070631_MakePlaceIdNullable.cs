using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrekkingGuideApp.Migrations
{
    /// <inheritdoc />
    public partial class MakePlaceIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_Places_PlaceId",
                table: "Itineraries");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "Itineraries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_Places_PlaceId",
                table: "Itineraries",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_Places_PlaceId",
                table: "Itineraries");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "Itineraries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_Places_PlaceId",
                table: "Itineraries",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
