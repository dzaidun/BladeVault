using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BladeVault.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCallLogForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_call_logs_performed_by_user_id",
                table: "call_logs",
                column: "performed_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_call_logs_orders_order_id",
                table: "call_logs",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_call_logs_users_customer_id",
                table: "call_logs",
                column: "customer_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_call_logs_users_performed_by_user_id",
                table: "call_logs",
                column: "performed_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_call_logs_orders_order_id",
                table: "call_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_call_logs_users_customer_id",
                table: "call_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_call_logs_users_performed_by_user_id",
                table: "call_logs");

            migrationBuilder.DropIndex(
                name: "IX_call_logs_performed_by_user_id",
                table: "call_logs");
        }
    }
}
