using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Canlı müzik, konserler ve festivaller", true, "Konser" },
                    { 2, "Tiyatro oyunları ve sahne sanatları", true, "Tiyatro" },
                    { 3, "Futbol, basketbol ve diğer spor müsabakaları", true, "Spor" },
                    { 4, "Vizyondaki filmler ve özel gösterimler", true, "Sinema" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Local), "admin@biletal.com", "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9", "Admin", "admin" },
                    { 2, new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Local), "esra@gmail.com", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", "User", "esra" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "AvailableSeats", "Capacity", "CategoryId", "Date", "Description", "ImageUrl", "Location", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 500, 500, 1, new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Local), "Tarkan, en sevilen şarkılarıyla Harbiye Açıkhava Sahnesi'nde hayranlarıyla buluşuyor.", "/images/tarkan.jpg", "Harbiye Cemil Topuzlu Açıkhava Tiyatrosu, İstanbul", 1500.00m, "Tarkan Harbiye Konseri" },
                    { 2, 300, 300, 2, new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Local), "Cem Yılmaz, yeni stand-up gösterisi ile Zorlu PSM'de kahkaha dolu bir gece sunuyor.", "/images/cemyilmaz.jpg", "Zorlu PSM - Turkcell Sahnesi, İstanbul", 1200.00m, "Cem Yılmaz - CMXXIV" },
                    { 3, 1000, 1000, 3, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Local), "Trendyol Süper Lig dev derbi heyecanı RAMS Park'ta yaşanıyor.", "/images/derbi.jpg", "RAMS Park, İstanbul", 2500.00m, "Galatasaray - Fenerbahçe Derbisi" },
                    { 4, 100, 100, 4, new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Local), "Christopher Nolan'ın efsanevi yapıtı Interstellar, dev perdede sinemaseverlerle buluşuyor.", "/images/interstellar.jpg", "Kadıköy Sineması, İstanbul", 200.00m, "Interstellar Özel Gösterim" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
