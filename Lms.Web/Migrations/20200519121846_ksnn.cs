using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kys.Web.Migrations
{
    public partial class ksnn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategori",
                columns: table => new
                {
                    KategoriKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori", x => x.KategoriKey);
                });

            migrationBuilder.CreateTable(
                name: "Uye",
                columns: table => new
                {
                    UyeKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KimlikNo = table.Column<string>(nullable: true),
                    Ad = table.Column<string>(nullable: true),
                    Soyad = table.Column<string>(nullable: true),
                    Telefon = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uye", x => x.UyeKey);
                });

            migrationBuilder.CreateTable(
                name: "UyeTip",
                columns: table => new
                {
                    UyeTipKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyeTip", x => x.UyeTipKey);
                });

            migrationBuilder.CreateTable(
                name: "Yayinevi",
                columns: table => new
                {
                    YayineviKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yayinevi", x => x.YayineviKey);
                });

            migrationBuilder.CreateTable(
                name: "Yazar",
                columns: table => new
                {
                    YazarKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(nullable: true),
                    YazarHakkinda = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yazar", x => x.YazarKey);
                });

            migrationBuilder.CreateTable(
                name: "Kitap",
                columns: table => new
                {
                    KitapKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YazarKey = table.Column<int>(nullable: false),
                    YayineviKey = table.Column<int>(nullable: false),
                    KitapDurumKey = table.Column<int>(nullable: false),
                    Ad = table.Column<string>(nullable: true),
                    Isbn = table.Column<string>(nullable: true),
                    DemirbasNo = table.Column<int>(nullable: false),
                    SayfaSayisi = table.Column<int>(nullable: false),
                    KitapHakkindaOzet = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitap", x => x.KitapKey);
                    table.ForeignKey(
                        name: "FK_Kitap_Yayinevi_YayineviKey",
                        column: x => x.YayineviKey,
                        principalTable: "Yayinevi",
                        principalColumn: "YayineviKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kitap_Yazar_YazarKey",
                        column: x => x.YazarKey,
                        principalTable: "Yazar",
                        principalColumn: "YazarKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KitapOdunc",
                columns: table => new
                {
                    KitapOduncKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KitapKey = table.Column<int>(nullable: false),
                    UyeKey = table.Column<int>(nullable: false),
                    AlisTarihi = table.Column<DateTime>(nullable: false),
                    GetirisTarihi = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitapOdunc", x => x.KitapOduncKey);
                    table.ForeignKey(
                        name: "FK_KitapOdunc_Kitap_KitapKey",
                        column: x => x.KitapKey,
                        principalTable: "Kitap",
                        principalColumn: "KitapKey",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KitapOdunc_Uye_UyeKey",
                        column: x => x.UyeKey,
                        principalTable: "Uye",
                        principalColumn: "UyeKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kitap_YayineviKey",
                table: "Kitap",
                column: "YayineviKey");

            migrationBuilder.CreateIndex(
                name: "IX_Kitap_YazarKey",
                table: "Kitap",
                column: "YazarKey");

            migrationBuilder.CreateIndex(
                name: "IX_KitapOdunc_KitapKey",
                table: "KitapOdunc",
                column: "KitapKey");

            migrationBuilder.CreateIndex(
                name: "IX_KitapOdunc_UyeKey",
                table: "KitapOdunc",
                column: "UyeKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kategori");

            migrationBuilder.DropTable(
                name: "KitapOdunc");

            migrationBuilder.DropTable(
                name: "UyeTip");

            migrationBuilder.DropTable(
                name: "Kitap");

            migrationBuilder.DropTable(
                name: "Uye");

            migrationBuilder.DropTable(
                name: "Yayinevi");

            migrationBuilder.DropTable(
                name: "Yazar");
        }
    }
}
