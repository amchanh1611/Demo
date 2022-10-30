using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Models.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "users");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birtday",
                table: "users",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birtday",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "users",
                type: "int",
                nullable: true);
        }
    }
}
