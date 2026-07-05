using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dondurmacf.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReinitAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicis",
                columns: table => new
                {
                    KullaniciNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicis", x => x.KullaniciNo);
                });

            migrationBuilder.CreateTable(
                name: "Uruns",
                columns: table => new
                {
                    UrunNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uruns", x => x.UrunNo);
                });

            migrationBuilder.CreateTable(
                name: "Turs",
                columns: table => new
                {
                    TurNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<int>(type: "int", nullable: false),
                    UrunNo = table.Column<int>(type: "int", nullable: false),
                    EkleyenKullaniciNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turs", x => x.TurNo);
                    table.ForeignKey(
                        name: "FK_Turs_Kullanicis_EkleyenKullaniciNo",
                        column: x => x.EkleyenKullaniciNo,
                        principalTable: "Kullanicis",
                        principalColumn: "KullaniciNo");
                    table.ForeignKey(
                        name: "FK_Turs_Uruns_UrunNo",
                        column: x => x.UrunNo,
                        principalTable: "Uruns",
                        principalColumn: "UrunNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kullanicis",
                columns: new[] { "KullaniciNo", "AdSoyad", "KullaniciAdi", "Sifre" },
                values: new object[,]
                {
                    { 1, "Esra Admin", "admin", "123" },
                    { 2, "Canan Personel", "user", "123" },
                    { 3, "Ömer Denetleyici", "supervisor", "123" },
                    { 4, "Aylin Kasiyer", "cashier", "123" }
                });

            migrationBuilder.InsertData(
                table: "Uruns",
                columns: new[] { "UrunNo", "Aciklama", "UrunAdi" },
                values: new object[,]
                {
                    { 1, "Geleneksel lezzetler", "Klasik Dondurmalar" },
                    { 2, "Taze meyvelerden üretilen dondurmalar", "Meyveli Dondurmalar" },
                    { 3, "Özel tarifler ve zengin içerikler", "Premium Dondurmalar" },
                    { 4, "Şekersiz ve bitkisel sütlü seçenekler", "Diyet & Vegan" },
                    { 5, "Dondurma eşliğinde sunulan lezzetler", "Tatlılar & Kup" }
                });

            migrationBuilder.InsertData(
                table: "Turs",
                columns: new[] { "TurNo", "EkleyenKullaniciNo", "Fiyat", "Tur", "UrunNo" },
                values: new object[,]
                {
                    { 1, 1, 50, "Sade Maraş", 1 },
                    { 2, 1, 55, "Çikolatalı", 1 },
                    { 3, 1, 60, "Çilekli", 2 },
                    { 4, 2, 60, "Limonlu", 2 },
                    { 5, 2, 75, "Antep Fıstıklı", 3 },
                    { 6, 2, 80, "Karamelli Bisküvili", 3 },
                    { 7, 3, 70, "Vanilyalı Kurabiyeli", 1 },
                    { 8, 3, 65, "Kavunlu", 2 },
                    { 9, 4, 65, "Böğürtlenli", 2 },
                    { 10, 1, 85, "Belçika Çikolatalı", 3 },
                    { 11, 4, 75, "Şekersiz Vanilyalı", 4 },
                    { 12, 3, 80, "Vegan Hindistan Cevizli", 4 },
                    { 13, 2, 95, "Dondurmalı Helva", 5 },
                    { 14, 1, 110, "Dondurmalı Sufle", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turs_EkleyenKullaniciNo",
                table: "Turs",
                column: "EkleyenKullaniciNo");

            migrationBuilder.CreateIndex(
                name: "IX_Turs_UrunNo",
                table: "Turs",
                column: "UrunNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Turs");

            migrationBuilder.DropTable(
                name: "Kullanicis");

            migrationBuilder.DropTable(
                name: "Uruns");
        }
    }
}
