using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MinIOKey",
                table: "Documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "DocumentAccesses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccesses_UserId1",
                table: "DocumentAccesses",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAccesses_Users_UserId1",
                table: "DocumentAccesses",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAccesses_Users_UserId1",
                table: "DocumentAccesses");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAccesses_UserId1",
                table: "DocumentAccesses");

            migrationBuilder.DropColumn(
                name: "MinIOKey",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "DocumentAccesses");
        }
    }
}
