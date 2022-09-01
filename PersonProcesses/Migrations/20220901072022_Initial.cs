using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonProcesses.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    UUId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Company = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.UUId);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformations",
                columns: table => new
                {
                    UUId = table.Column<Guid>(type: "uuid", nullable: false),
                    InformationType = table.Column<int>(type: "integer", nullable: false),
                    InformationContent = table.Column<string>(type: "text", nullable: false),
                    PersonUUId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformations", x => x.UUId);
                    table.ForeignKey(
                        name: "FK_ContactInformations_Persons_PersonUUId",
                        column: x => x.PersonUUId,
                        principalTable: "Persons",
                        principalColumn: "UUId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformations_PersonUUId",
                table: "ContactInformations",
                column: "PersonUUId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInformations");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
