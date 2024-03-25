using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProjesi1.Migrations
{
    /// <inheritdoc />
    public partial class stokdeneme2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "kitapadedi",
                table: "Kiralamalar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "kitapadedi",
                table: "Kiralamalar");
        }
    }
}
