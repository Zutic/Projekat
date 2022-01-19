using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekat.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BrojStanovnika = table.Column<int>(type: "int", nullable: false),
                    BrojNekretnina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kompanije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kompanije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stanovnici",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Novac = table.Column<double>(type: "float", nullable: false),
                    GradStanovanjaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanovnici", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stanovnici_Gradovi_GradStanovanjaID",
                        column: x => x.GradStanovanjaID,
                        principalTable: "Gradovi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nekretnina",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    LokacijaID = table.Column<int>(type: "int", nullable: true),
                    GraditeljID = table.Column<int>(type: "int", nullable: true),
                    PocetnaCena = table.Column<double>(type: "float", nullable: false),
                    Slika = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nekretnina", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Nekretnina_Gradovi_LokacijaID",
                        column: x => x.LokacijaID,
                        principalTable: "Gradovi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nekretnina_Kompanije_GraditeljID",
                        column: x => x.GraditeljID,
                        principalTable: "Kompanije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ugovori",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KupacID = table.Column<int>(type: "int", nullable: true),
                    ProdavacID = table.Column<int>(type: "int", nullable: true),
                    Procenat = table.Column<int>(type: "int", nullable: false),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    ObjekatID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ugovori", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ugovori_Nekretnina_ObjekatID",
                        column: x => x.ObjekatID,
                        principalTable: "Nekretnina",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ugovori_Stanovnici_KupacID",
                        column: x => x.KupacID,
                        principalTable: "Stanovnici",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ugovori_Stanovnici_ProdavacID",
                        column: x => x.ProdavacID,
                        principalTable: "Stanovnici",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UgovoriKompanija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdavacID = table.Column<int>(type: "int", nullable: true),
                    KupacID = table.Column<int>(type: "int", nullable: true),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    NekretninaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgovoriKompanija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UgovoriKompanija_Kompanije_ProdavacID",
                        column: x => x.ProdavacID,
                        principalTable: "Kompanije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UgovoriKompanija_Nekretnina_NekretninaFK",
                        column: x => x.NekretninaFK,
                        principalTable: "Nekretnina",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgovoriKompanija_Stanovnici_KupacID",
                        column: x => x.KupacID,
                        principalTable: "Stanovnici",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nekretnina_GraditeljID",
                table: "Nekretnina",
                column: "GraditeljID");

            migrationBuilder.CreateIndex(
                name: "IX_Nekretnina_LokacijaID",
                table: "Nekretnina",
                column: "LokacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Stanovnici_GradStanovanjaID",
                table: "Stanovnici",
                column: "GradStanovanjaID");

            migrationBuilder.CreateIndex(
                name: "IX_Ugovori_KupacID",
                table: "Ugovori",
                column: "KupacID");

            migrationBuilder.CreateIndex(
                name: "IX_Ugovori_ObjekatID",
                table: "Ugovori",
                column: "ObjekatID");

            migrationBuilder.CreateIndex(
                name: "IX_Ugovori_ProdavacID",
                table: "Ugovori",
                column: "ProdavacID");

            migrationBuilder.CreateIndex(
                name: "IX_UgovoriKompanija_KupacID",
                table: "UgovoriKompanija",
                column: "KupacID");

            migrationBuilder.CreateIndex(
                name: "IX_UgovoriKompanija_NekretninaFK",
                table: "UgovoriKompanija",
                column: "NekretninaFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UgovoriKompanija_ProdavacID",
                table: "UgovoriKompanija",
                column: "ProdavacID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ugovori");

            migrationBuilder.DropTable(
                name: "UgovoriKompanija");

            migrationBuilder.DropTable(
                name: "Nekretnina");

            migrationBuilder.DropTable(
                name: "Stanovnici");

            migrationBuilder.DropTable(
                name: "Kompanije");

            migrationBuilder.DropTable(
                name: "Gradovi");
        }
    }
}
