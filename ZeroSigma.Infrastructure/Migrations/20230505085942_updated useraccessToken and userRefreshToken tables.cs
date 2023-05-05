using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroSigma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateduseraccessTokenanduserRefreshTokentables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userID",
                table: "UsersRefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "userID",
                table: "UsersRefreshToken",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
