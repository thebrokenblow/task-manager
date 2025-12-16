using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Persistence.Data.Configurations;

/// <summary>
/// Конфигурация таблицы документов.
/// </summary>
public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    /// <summary>
    /// Настраивает таблицу документов.
    /// </summary>
    /// <param name="builder">Строитель конфигурации сущности Document.</param>
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents",
            tableBuilder =>
                tableBuilder.HasComment("Таблица для хранения документов системы TaskManager"));

        builder.HasKey(document => document.Id);

        builder.Property(document => document.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn()
            .HasComment("Уникальный идентификатор документа.");

        builder.Property(document => document.CreatedByEmployeeId)
            .HasColumnName("created_by_employee_id")
            .HasComment("Идентификатор сотрудника, который создал документ.")
            .IsRequired();

        builder.HasOne(document => document.CreatedByEmployee)
            .WithMany()
            .HasForeignKey(document => document.CreatedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(document => document.OutgoingDocumentNumberInputDocument)
            .HasColumnName("outgoing_document_number_input_document")
            .HasComment("Исходный номер документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired(false);

        builder.Property(document => document.SourceDocumentDateInputDocument)
            .HasColumnName("source_document_date_input_document")
            .HasColumnType("date")
            .HasComment("Дата исходного документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired(false);

        builder.Property(document => document.CustomerInputDocument)
            .HasColumnName("customer_input_document")
            .HasComment("Заказчик. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired(false);

        builder.Property(document => document.DocumentSummaryInputDocument)
            .HasColumnName("document_summary_input_document")
            .HasComment("Краткое содержание документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired(false);

        builder.Property(document => document.IsExternalDocumentInputDocument)
            .HasColumnName("is_external_document_input_document")
            .HasComment("Признак внешнего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired();

        builder.Property(document => document.IncomingDocumentNumberInputDocument)
            .HasColumnName("incoming_document_number_input_document")
            .HasComment("Входящий номер документа ВХ(46 ЦНИИ). Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired();

        builder.Property(document => document.IncomingDocumentDateInputDocument)
            .HasColumnName("incoming_document_date_input_document")
            .HasColumnType("date")
            .HasComment("Дата входящего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired();

        builder.Property(document => document.IncomingDocumentDateInputDocument)
            .HasColumnName("incoming_document_date_input_document")
            .HasColumnType("date")
            .HasComment("Дата входящего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired();

        builder.Property(document => document.ResponsibleDepartmentInputDocument)
            .HasColumnName("responsible_department_input_document")
            .HasComment("ответственный отдел входящего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired(false);

        builder.Property(document => document.ResponsibleDepartmentsInputDocument)
            .HasColumnName("responsible_departments_input_document")
            .HasComment("Привлеченные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired(false);

        builder.Property(document => document.TaskDueDateInputDocument)
            .HasColumnName("task_due_date_input_document")
            .HasColumnType("date")
            .HasComment("Срок выполнения задачи. Входные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired();

        builder.Property(document => document.IdResponsibleEmployeeInputDocument)
            .HasColumnName("id_responsible_employee_input_document")
            .HasComment("Идентификатор ответственного сотрудника. Входные данные документа. Заполняет исполнитель.")
            .IsRequired(false);

        builder.HasOne(document => document.ResponsibleEmployeeInputDocument)
            .WithMany()
            .HasForeignKey(document => document.IdResponsibleEmployeeInputDocument)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.Property(document => document.IsExternalDocumentOutputDocument)
            .HasColumnName("is_external_document_output_document")
            .HasComment("Признак внешнего документа. Выходные данные документа. Заполняет исполнитель.")
            .IsRequired();

        builder.Property(document => document.OutgoingDocumentNumberOutputDocument)
            .HasColumnName("outgoing_document_number_output_document")
            .HasComment("Исходящий номер документа Исх(46 ЦНИИ). Выходные данные документа. Заполняет исполнитель.")
            .IsRequired(false);

        builder.Property(document => document.OutgoingDocumentDateOutputDocument)
            .HasColumnName("outgoing_document_date_output_document")
            .HasColumnType("date")
            .HasComment("Дата исходящего документа. Выходные данные документа. Заполняет исполнитель.")
            .IsRequired(false);

        builder.Property(document => document.RecipientOutputDocument)
            .HasColumnName("recipient_output_document")
            .HasComment("Получатель. Выходные данные документа. Заполняет исполнитель.")
            .IsRequired(false);

        builder.Property(document => document.DocumentSummaryOutputDocument)
            .HasColumnName("document_summary_output_document")
            .HasComment("Краткое содержание документа. Выходные данные документа. Заполняет исполнитель.")
            .IsRequired(false);

        builder.Property(document => document.IsUnderControl)
            .HasColumnName("is_under_control")
            .HasComment("Признак нахождения задачи на контроле. Выходные данные документа. Заполняет хозяин записи (делопроизводитель).")
            .IsRequired();

        builder.Property(document => document.IsCompleted)
            .HasColumnName("is_completed")
            .HasComment("Признак завершения задачи. Заполняет исполнитель.")
            .IsRequired();

        builder.Property(document => document.LastEditedByEmployeeId)
            .HasColumnName("last_edited_by_employee_id")
            .HasComment("Идентификатор сотрудника, который последним редактировал документ.")
            .IsRequired(false);

        builder.HasOne(document => document.LastEditedByEmployee)
            .WithMany()
            .HasForeignKey(document => document.LastEditedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.Property(document => document.LastEditedDateTime)
            .HasColumnName("last_edited_date_time")
            .HasColumnType("timestamp without time zone")
            .HasComment("Дата и время последнего редактирования документа.")
            .IsRequired(false);

        builder.Property(document => document.RemovedByEmployeeId)
            .HasColumnName("removed_by_employee_id")
            .HasComment("Идентификатор сотрудника, который удалил запись.")
            .IsRequired(false);

        builder.HasOne(document => document.RemovedByEmployee)
            .WithMany()
            .HasForeignKey(document => document.RemovedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.Property(document => document.RemoveDateTime)
            .HasColumnName("remove_date_time")
            .HasColumnType("timestamp without time zone")
            .HasComment("Дата удаления документа.")
            .IsRequired(false);
    }
}