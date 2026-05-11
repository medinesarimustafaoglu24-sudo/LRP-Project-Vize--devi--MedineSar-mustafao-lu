using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRP_Project_Vize_MedineSarımustafaoğlu.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserComputerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LabId",
                table: "Computers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Computers_LabId",
                table: "Computers",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_Computers_StudentId",
                table: "Computers",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_Labs_LabId",
                table: "Computers",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_Users_StudentId",
                table: "Computers",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_Labs_LabId",
                table: "Computers");

            migrationBuilder.DropForeignKey(
                name: "FK_Computers_Users_StudentId",
                table: "Computers");

            migrationBuilder.DropIndex(
                name: "IX_Computers_LabId",
                table: "Computers");

            migrationBuilder.DropIndex(
                name: "IX_Computers_StudentId",
                table: "Computers");

            migrationBuilder.AlterColumn<int>(
                name: "LabId",
                table: "Computers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
