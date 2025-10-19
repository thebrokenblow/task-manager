using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskManager.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "employees",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                full_name = table.Column<string>(type: "text", nullable: false),
                department = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_employees", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "documents",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                source_outgoing_document_number = table.Column<string>(type: "text", nullable: false),
                source_outgoing_document_date = table.Column<DateOnly>(type: "date", nullable: false),
                source_customer = table.Column<string>(type: "text", nullable: false),
                source_task_text = table.Column<string>(type: "text", nullable: false),
                source_is_external = table.Column<bool>(type: "boolean", nullable: false),
                source_output_document_number = table.Column<string>(type: "text", nullable: false),
                source_output_document_date = table.Column<DateOnly>(type: "date", nullable: false),
                source_due_date = table.Column<DateOnly>(type: "date", nullable: false),
                source_responsible_employee_id = table.Column<int>(type: "integer", nullable: false),
                output_outgoing_number = table.Column<string>(type: "text", nullable: true),
                output_outgoing_date = table.Column<DateOnly>(type: "date", nullable: true),
                output_sent_to = table.Column<string>(type: "text", nullable: true),
                output_transferred_in_work_order = table.Column<string>(type: "text", nullable: true),
                output_response_submission_mark = table.Column<string>(type: "text", nullable: true),
                is_under_control = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_documents", x => x.id);
                table.ForeignKey(
                    name: "FK_documents_employees_source_responsible_employee_id",
                    column: x => x.source_responsible_employee_id,
                    principalTable: "employees",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_documents_source_responsible_employee_id",
            table: "documents",
            column: "source_responsible_employee_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "documents");

        migrationBuilder.DropTable(
            name: "employees");
    }
}
