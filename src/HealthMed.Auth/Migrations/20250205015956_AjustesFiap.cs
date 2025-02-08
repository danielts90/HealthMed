using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Auth.Migrations
{
    /// <inheritdoc />
    public partial class AjustesFiap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondaryLogin",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryLogin",
                table: "Users");
        }
    }
}
