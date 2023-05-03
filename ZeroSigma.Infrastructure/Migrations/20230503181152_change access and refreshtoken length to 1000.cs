using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroSigma.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeaccessandrefreshtokenlengthto1000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "UsersRefreshToken",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(600)",
                oldMaxLength: 600);

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "UsersAccessToken",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(600)",
                oldMaxLength: 600);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "UsersRefreshToken",
                type: "varchar(600)",
                maxLength: 600,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "UsersAccessToken",
                type: "varchar(600)",
                maxLength: 600,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
