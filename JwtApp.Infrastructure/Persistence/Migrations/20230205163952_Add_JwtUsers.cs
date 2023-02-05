using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJwtUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwtUsers",
                columns: table => new
                {
                    JwtUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtUsers", x => x.JwtUserId);
                    table.ForeignKey(
                        name: "FK_JwtUsers_ApplicationUsers_JwtUserId",
                        column: x => x.JwtUserId,
                        principalSchema: "auth",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwtUsers");
        }
    }
}
