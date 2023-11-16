using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace orderManageApp.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomPatronymic",
                table: "Order_positions");

            migrationBuilder.DropColumn(
                name: "CustomSurname",
                table: "Order_positions");

            migrationBuilder.RenameColumn(
                name: "IsRemoved",
                table: "Orders",
                newName: "Removed");

            migrationBuilder.RenameColumn(
                name: "IsRemoved",
                table: "Order_positions",
                newName: "Removed");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProductSku",
                table: "Order_positions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductSku",
                table: "Order_positions");

            migrationBuilder.RenameColumn(
                name: "Removed",
                table: "Orders",
                newName: "IsRemoved");

            migrationBuilder.RenameColumn(
                name: "Removed",
                table: "Order_positions",
                newName: "IsRemoved");

            migrationBuilder.AddColumn<string>(
                name: "CustomPatronymic",
                table: "Order_positions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomSurname",
                table: "Order_positions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
