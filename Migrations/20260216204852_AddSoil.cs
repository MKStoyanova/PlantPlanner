using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlantPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddSoil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoilId",
                table: "Plants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Soils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soils", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Soils",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Standard" },
                    { 2, "For carnivorous plants" },
                    { 3, "For orchids" },
                    { 4, "For cacti/succulents" },
                    { 5, "For flowering" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plants_SoilId",
                table: "Plants",
                column: "SoilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Soils_SoilId",
                table: "Plants",
                column: "SoilId",
                principalTable: "Soils",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Soils_SoilId",
                table: "Plants");

            migrationBuilder.DropTable(
                name: "Soils");

            migrationBuilder.DropIndex(
                name: "IX_Plants_SoilId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "SoilId",
                table: "Plants");
        }
    }
}
