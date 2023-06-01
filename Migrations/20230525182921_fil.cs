using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroExpressAPI.Migrations
{
    /// <inheritdoc />
    public partial class fil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThirdDimentionPicture",
                table: "Products",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecondDimentionPicture",
                table: "Products",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ForthDimentionPicture",
                table: "Products",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstDimentionPicture",
                table: "Products",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 5, 25, 19, 29, 21, 100, DateTimeKind.Local).AddTicks(2491), "$2b$10$g/pHoM0xbbZrImtT71rioeRfKVCSLaEwdOuZrUnioqXf4tuZ7Ltv2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ThirdDimentionPicture",
                table: "Products",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "SecondDimentionPicture",
                table: "Products",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ForthDimentionPicture",
                table: "Products",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "FirstDimentionPicture",
                table: "Products",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 5, 24, 15, 32, 4, 539, DateTimeKind.Local).AddTicks(350), "$2b$10$6cvEtJWRjayJyIWMVYxoiuAyvUU1zwMthQuJhwDjFm.juboadMzFK" });
        }
    }
}
