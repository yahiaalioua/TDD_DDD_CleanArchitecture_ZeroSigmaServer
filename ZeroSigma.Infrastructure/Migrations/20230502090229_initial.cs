using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroSigma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    FullName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "varchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersAccessToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    AccessToken = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsExpired = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAccessToken", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersRefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    userID = table.Column<Guid>(type: "char(36)", nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsExpired = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRefreshToken", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserID = table.Column<Guid>(type: "char(36)", nullable: false),
                    AccessTokenID = table.Column<Guid>(type: "char(36)", nullable: false),
                    RefreshTokenID = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersAccess_UsersAccessToken_AccessTokenID",
                        column: x => x.AccessTokenID,
                        principalTable: "UsersAccessToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersAccess_UsersRefreshToken_RefreshTokenID",
                        column: x => x.RefreshTokenID,
                        principalTable: "UsersRefreshToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersAccess_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAccess_AccessTokenID",
                table: "UsersAccess",
                column: "AccessTokenID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAccess_RefreshTokenID",
                table: "UsersAccess",
                column: "RefreshTokenID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAccess_UserID",
                table: "UsersAccess",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersAccess");

            migrationBuilder.DropTable(
                name: "UsersAccessToken");

            migrationBuilder.DropTable(
                name: "UsersRefreshToken");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
