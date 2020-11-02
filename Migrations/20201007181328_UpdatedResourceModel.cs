using Microsoft.EntityFrameworkCore.Migrations;

namespace NeverLeftBehind.Migrations
{
    public partial class UpdatedResourceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "resources",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_responses_ResourceId",
                table: "responses",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_responses_resources_ResourceId",
                table: "responses",
                column: "ResourceId",
                principalTable: "resources",
                principalColumn: "ResourceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_responses_resources_ResourceId",
                table: "responses");

            migrationBuilder.DropIndex(
                name: "IX_responses_ResourceId",
                table: "responses");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "resources",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
