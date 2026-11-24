using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentMonitoring.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPredictionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Predictions");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Predictions");

            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "Predictions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
