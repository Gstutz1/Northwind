using Microsoft.EntityFrameworkCore.Migrations;

namespace Northwind.Migrations
{
    public partial class Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Products_ProductID",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Discounts",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "DiscountID",
                table: "Discounts",
                newName: "DiscountId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_ProductID",
                table: "Discounts",
                newName: "IX_Discounts_ProductId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Customers",
                newName: "CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Products_ProductId",
                table: "Discounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Products_ProductId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Discounts",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "Discounts",
                newName: "DiscountID");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_ProductId",
                table: "Discounts",
                newName: "IX_Discounts_ProductID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Customers",
                newName: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Products_ProductID",
                table: "Discounts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
