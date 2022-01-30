using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModalBuilderUtil.Migrations
{
    public partial class AddUserIdToDbModal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DBModalId",
                table: "Modals",
                newName: "DbModalId");

            migrationBuilder.AddColumn<ulong>(
                name: "UserID",
                table: "Modals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Modals");

            migrationBuilder.RenameColumn(
                name: "DbModalId",
                table: "Modals",
                newName: "DBModalId");
        }
    }
}
