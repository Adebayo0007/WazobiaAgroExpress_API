using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroExpressAPI.Migrations
{
    /// <inheritdoc />
    public partial class initi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 7, 25, 11, 59, 43, 688, DateTimeKind.Local).AddTicks(4660), "$2b$10$LR6XnY4.YI8rn1ph9XrkLeOx3diylFmIQNuVcRA4hypSrUcjqehIK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 7, 25, 11, 57, 40, 706, DateTimeKind.Local).AddTicks(1308), "$2b$10$K7No85q6UDbf8hDJbxRI2.gDB8EsTQ0gqGnqkIaH/ilp.OEJYSgPO" });
        }
    }
}
