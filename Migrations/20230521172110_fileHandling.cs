using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroExpressAPI.Migrations
{
    /// <inheritdoc />
    public partial class fileHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    FullAddress = table.Column<string>(type: "longtext", nullable: true),
                    LocalGovernment = table.Column<string>(type: "longtext", nullable: true),
                    State = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    FarmerEmail = table.Column<string>(type: "longtext", nullable: true),
                    BuyerEmail = table.Column<string>(type: "longtext", nullable: true),
                    DateCreated = table.Column<string>(type: "longtext", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    ProfilePicture = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    AddressId = table.Column<string>(type: "varchar(255)", nullable: true),
                    Gender = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    Password = table.Column<string>(type: "longtext", nullable: true),
                    Role = table.Column<string>(type: "longtext", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRegistered = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Haspaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Due = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buyers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true),
                    Ranking = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farmers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    FarmerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    FirstDimentionPicture = table.Column<byte[]>(type: "longblob", nullable: true),
                    SecondDimentionPicture = table.Column<byte[]>(type: "longblob", nullable: true),
                    ThirdDimentionPicture = table.Column<byte[]>(type: "longblob", nullable: true),
                    ForthDimentionPicture = table.Column<byte[]>(type: "longblob", nullable: true),
                    ProductName = table.Column<string>(type: "longtext", nullable: true),
                    FarmerUserName = table.Column<string>(type: "longtext", nullable: true),
                    FarmerEmail = table.Column<string>(type: "longtext", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
                    Measurement = table.Column<string>(type: "longtext", nullable: true),
                    AvailabilityDateFrom = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AvailabilityDateTo = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ProductLocalGovernment = table.Column<string>(type: "longtext", nullable: true),
                    ProductState = table.Column<string>(type: "longtext", nullable: true),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FarmerRank = table.Column<int>(type: "int", nullable: false),
                    ThumbUp = table.Column<int>(type: "int", nullable: false),
                    ThumbDown = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RequestedProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    FarmerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    BuyerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    BuyerEmail = table.Column<string>(type: "longtext", nullable: true),
                    BuyerPhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    BuyerLocalGovernment = table.Column<string>(type: "longtext", nullable: true),
                    ProductName = table.Column<string>(type: "longtext", nullable: true),
                    FarmerName = table.Column<string>(type: "longtext", nullable: true),
                    FarmerNumber = table.Column<string>(type: "longtext", nullable: true),
                    Quantity = table.Column<double>(type: "double", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false),
                    OrderStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAccepted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDelivered = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NotDelivered = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Haspaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FarmerEmail = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedProducts_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestedProducts_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "FullAddress", "LocalGovernment", "State" },
                values: new object[] { "cc7578e3-52a9-49e9-9788-2da54df19f38", "10,Abayomi street,Ipaja,lagos", "Alimosho", "Lagos" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "DateCreated", "DateModified", "Due", "Email", "Gender", "Haspaid", "IsActive", "IsRegistered", "Name", "Password", "PhoneNumber", "ProfilePicture", "Role", "UserName" },
                values: new object[] { "37846734-732e-4149-8cec-6f43d1eb3f60", "cc7578e3-52a9-49e9-9788-2da54df19f38", new DateTime(2023, 5, 21, 18, 21, 10, 247, DateTimeKind.Local).AddTicks(2565), null, true, "tijaniadebayoabdllahi@gmail.com", "Male", true, true, true, "Adebayo Addullah", "$2b$10$11s7SA72IZh1xxraUNfRT.6xbGHbDBwfRtbIa307ixS1ixwKEK5L2", "08087054632", null, "Admin", "Modrator" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "UserId" },
                values: new object[] { "37846734-732e-4149-8cec-6f43d1eb3f60", "37846734-732e-4149-8cec-6f43d1eb3f60" });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_UserId",
                table: "Buyers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_UserId",
                table: "Farmers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedProducts_BuyerId",
                table: "RequestedProducts",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedProducts_FarmerId",
                table: "RequestedProducts",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RequestedProducts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "Farmers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
