using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModalBuilderUtil.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modals",
                columns: table => new
                {
                    DBModalId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    CustomId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modals", x => x.DBModalId);
                });

            migrationBuilder.CreateTable(
                name: "ActionRows",
                columns: table => new
                {
                    DbActionRowId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DBModalId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionRows", x => x.DbActionRowId);
                    table.ForeignKey(
                        name: "FK_ActionRows_Modals_DBModalId",
                        column: x => x.DBModalId,
                        principalTable: "Modals",
                        principalColumn: "DBModalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    DbComponentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DbActionRowId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomId = table.Column<string>(type: "TEXT", nullable: true),
                    Label = table.Column<string>(type: "TEXT", nullable: true),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: true),
                    Placeholder = table.Column<string>(type: "TEXT", nullable: true),
                    Min = table.Column<int>(type: "INTEGER", nullable: true),
                    Max = table.Column<int>(type: "INTEGER", nullable: true),
                    ButtonStyle = table.Column<int>(type: "INTEGER", nullable: false),
                    Emote = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    TextInputStyle = table.Column<int>(type: "INTEGER", nullable: false),
                    Required = table.Column<bool>(type: "INTEGER", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.DbComponentId);
                    table.ForeignKey(
                        name: "FK_Components_ActionRows_DbActionRowId",
                        column: x => x.DbActionRowId,
                        principalTable: "ActionRows",
                        principalColumn: "DbActionRowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectOptions",
                columns: table => new
                {
                    DbSelectOptionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Emote = table.Column<string>(type: "TEXT", nullable: true),
                    Default = table.Column<bool>(type: "INTEGER", nullable: true),
                    DbComponentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectOptions", x => x.DbSelectOptionId);
                    table.ForeignKey(
                        name: "FK_SelectOptions_Components_DbComponentId",
                        column: x => x.DbComponentId,
                        principalTable: "Components",
                        principalColumn: "DbComponentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionRows_DBModalId",
                table: "ActionRows",
                column: "DBModalId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_DbActionRowId",
                table: "Components",
                column: "DbActionRowId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectOptions_DbComponentId",
                table: "SelectOptions",
                column: "DbComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectOptions");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "ActionRows");

            migrationBuilder.DropTable(
                name: "Modals");
        }
    }
}
