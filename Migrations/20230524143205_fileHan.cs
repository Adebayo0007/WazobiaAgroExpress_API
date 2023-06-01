using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroExpressAPI.Migrations
{
    /// <inheritdoc />
    public partial class fileHan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Haspaid", "Password" },
                values: new object[] { new DateTime(2023, 5, 24, 15, 32, 4, 539, DateTimeKind.Local).AddTicks(350), true, "$2b$10$6cvEtJWRjayJyIWMVYxoiuAyvUU1zwMthQuJhwDjFm.juboadMzFK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Haspaid", "Password" },
                values: new object[] { new DateTime(2023, 5, 24, 15, 28, 23, 777, DateTimeKind.Local).AddTicks(3952), false, "$2b$10$dUPuff9WJwLpFQp0lCzAFeLSaxHkgaqmDWCojsOHtdzWIqo/sPeHi" });
        }
    }
}
