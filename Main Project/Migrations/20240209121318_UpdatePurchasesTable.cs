using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Main_Project.Migrations
{
    public partial class UpdatePurchasesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Purchases",
                newName: "FullName");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Purchases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Purchases",
                newName: "Username");
        }
    }
}
