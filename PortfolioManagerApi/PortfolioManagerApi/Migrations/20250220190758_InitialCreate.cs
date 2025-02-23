using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioManagerApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        // Tworzy wszystkie tabele i dodaje dane do bazy przy migracji w górę
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tworzenie tabeli "Projects"
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepositoryUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DemoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            // Tworzenie tabeli "Technologies"
            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.Id);
                });

            // Tworzenie tabeli pośredniej "ProjectTechnologies"
            migrationBuilder.CreateTable(
                name: "ProjectTechnologies",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnologyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    // Ustawienie klucza głównego dla tabeli łączącej projekty i technologie
                    table.PrimaryKey("PK_ProjectTechnologies", x => new { x.ProjectId, x.TechnologyId });
                    // Ustanowienie relacji z tabelą "Projects"
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    // Ustanowienie relacji z tabelą "Technologies"
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Tworzenie indeksu na kolumnie "TechnologyId" w tabeli "ProjectTechnologies"
            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechnologies_TechnologyId",
                table: "ProjectTechnologies",
                column: "TechnologyId");

            // Dodawanie przykładowych technologii
            migrationBuilder.InsertData(
                table: "Technologies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "C#" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "JavaScript" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Python" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Java" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "TypeScript" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "React" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "Angular" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "Node.js" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "SQL" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "HTML/CSS" }
                });

            // Dodawanie przykładowych projektów
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Title", "Description", "StartDate", "EndDate", "Status", "RepositoryUrl", "DemoUrl" },
                values: new object[,]
                {
                    {
                        new Guid("10000000-0000-0000-0000-000000000001"),
                        "E-commerce Platform",
                        "Sklep internetowy z użyciem ASP.NET Core i React",
                        new DateTime(2023, 1, 1),
                        new DateTime(2023, 4, 1),
                        "Zakończony",
                        "https://github.com/ecommerce",
                        "https://ecommerce.demo.com"
                    },
                    {
                        new Guid("10000000-0000-0000-0000-000000000002"),
                        "Portal społecznościowy",
                        "Aplikacja społecznościowa w Angular i Node.js",
                        new DateTime(2023, 2, 1),
                        new DateTime(2023, 5, 1),
                        "W trakcie",
                        "https://github.com/social",
                        "https://social.demo.com"
                    }
                    // Dodaj kolejne 8 projektów...
                });

            // Dodawanie powiązań projekt-technologia
            migrationBuilder.InsertData(
                table: "ProjectTechnologies",
                columns: new[] { "ProjectId", "TechnologyId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") }, // C#
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000006") }, // React
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000002") }, // JavaScript
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000007") }, // Angular
                    // Dodaj więcej powiązań...
                });
        }

        // Metoda, która cofa zmiany przy migracji w dół (usuwanie tabel i danych)
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Usunięcie tabeli pośredniej "ProjectTechnologies"
            migrationBuilder.DropTable(
                name: "ProjectTechnologies");

            // Usunięcie tabeli "Projects"
            migrationBuilder.DropTable(
                name: "Projects");

            // Usunięcie tabeli "Technologies"
            migrationBuilder.DropTable(
                name: "Technologies");
        }
    }
}
