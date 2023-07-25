using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroExpressAPI.Migrations
{
    /// <inheritdoc />
    public partial class initials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password", "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { new DateTime(2023, 7, 25, 11, 57, 40, 706, DateTimeKind.Local).AddTicks(1308), "$2b$10$K7No85q6UDbf8hDJbxRI2.gDB8EsTQ0gqGnqkIaH/ilp.OEJYSgPO", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 5, 25, 19, 29, 21, 100, DateTimeKind.Local).AddTicks(2491), "$2b$10$g/pHoM0xbbZrImtT71rioeRfKVCSLaEwdOuZrUnioqXf4tuZ7Ltv2" });
        }
    }
}
