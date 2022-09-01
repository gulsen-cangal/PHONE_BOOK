using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReportStatus = table.Column<int>(type: "integer", nullable: false),
                    ReportPath = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.UUID);
                });

            migrationBuilder.CreateTable(
                name: "ReportDetail",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    PersonCount = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumberCount = table.Column<int>(type: "integer", nullable: false),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDetail", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_ReportDetail_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportDetail_ReportId",
                table: "ReportDetail",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportDetail");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
