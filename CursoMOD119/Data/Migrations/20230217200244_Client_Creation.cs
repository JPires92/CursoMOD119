using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoMOD119.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSale_Sales_SalesID",
                table: "ItemSale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "Sale");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sale",
                table: "Sale",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSale_Sale_SalesID",
                table: "ItemSale",
                column: "SalesID",
                principalTable: "Sale",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSale_Sale_SalesID",
                table: "ItemSale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sale",
                table: "Sale");

            migrationBuilder.RenameTable(
                name: "Sale",
                newName: "Sales");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSale_Sales_SalesID",
                table: "ItemSale",
                column: "SalesID",
                principalTable: "Sales",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
