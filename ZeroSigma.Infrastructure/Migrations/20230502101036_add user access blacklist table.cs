using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroSigma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adduseraccessblacklisttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersAccessBlackLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    RefreshTokenID = table.Column<Guid>(type: "char(36)", nullable: false),
                    RevokedRefreshTokens = table.Column<string>(type: "varchar(7000)", maxLength: 7000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAccessBlackLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersAccessBlackLists_UsersRefreshToken_RefreshTokenID",
                        column: x => x.RefreshTokenID,
                        principalTable: "UsersRefreshToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAccessBlackLists_RefreshTokenID",
                table: "UsersAccessBlackLists",
                column: "RefreshTokenID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersAccessBlackLists");
        }
    }
}
