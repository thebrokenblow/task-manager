using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class popkek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "responsible_departments_input_document",
                table: "documents",
                type: "text",
                nullable: true,
                comment: "Привлеченные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Ответственные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).");

            migrationBuilder.AddColumn<string>(
                name: "responsible_department_input_document",
                table: "documents",
                type: "text",
                nullable: true,
                comment: "ответственный отдел входящего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "responsible_department_input_document",
                table: "documents");

            migrationBuilder.AlterColumn<string>(
                name: "responsible_departments_input_document",
                table: "documents",
                type: "text",
                nullable: true,
                comment: "Ответственные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Привлеченные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).");
        }
    }
}
