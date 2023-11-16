using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace orderManageApp.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderNumber = table.Column<string>(type: "text", nullable: false),
                    CustomName = table.Column<string>(type: "text", nullable: false),
                    CustomSurname = table.Column<string>(type: "text", nullable: false),
                    CustomPatronymic = table.Column<string>(type: "text", nullable: true),
                    SumMoney = table.Column<decimal>(type: "numeric", nullable: false),
                    IsRemoved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Order_positions",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomSurname = table.Column<string>(type: "text", nullable: false),
                    CustomPatronymic = table.Column<string>(type: "text", nullable: true),
                    SumMoney = table.Column<decimal>(type: "numeric", nullable: false),
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_positions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_positions_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_positions_OrderID",
                table: "Order_positions",
                column: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_positions");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
