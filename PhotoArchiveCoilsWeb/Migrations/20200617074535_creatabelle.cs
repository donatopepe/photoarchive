using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoArchiveCoilsWeb.Migrations
{
    public partial class creatabelle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhotoArchives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER ROWGUIDCOL", nullable: false),
                    Cam = table.Column<string>(nullable: true),
                    File = table.Column<byte[]>(type: "VARBINARY(MAX) FILESTREAM", nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Parent = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(nullable: false),
                    UpdatedTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoArchives", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoArchives");
        }
    }
}
