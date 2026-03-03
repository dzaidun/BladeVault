using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BladeVault.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderItemProductRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_products_ProductId1",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "IX_order_items_ProductId1",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "order_items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "order_items",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_items_ProductId1",
                table: "order_items",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_products_ProductId1",
                table: "order_items",
                column: "ProductId1",
                principalTable: "products",
                principalColumn: "id");
        }
    }
}
