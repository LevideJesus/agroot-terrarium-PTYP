using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrootTerrarium.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddZoneType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZoneType",
                table: "Zones",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoneType",
                table: "Zones");
        }
    }
}
