using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoMOD119.Data.Migrations
{
    /// <inheritdoc />
    public partial class Sale_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientID",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ClientID",
                table: "Sales",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Clients_ClientID",
                table: "Sales",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Clients_ClientID",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ClientID",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "Sales");
        }
    }
}
