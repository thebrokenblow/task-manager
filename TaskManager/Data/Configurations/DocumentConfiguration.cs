using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Models;

namespace TaskManager.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(document => document.Id);

        builder
            .Property(document => document.Id)
            .HasColumnName("id");

        builder
            .Property(document => document.SourceOutgoingDocumentNumber)
            .IsRequired()
            .HasColumnName("source_outgoing_document_number");

        builder
            .Property(document => document.SourceOutgoingDocumentDate)
            .IsRequired()
            .HasColumnName("source_outgoing_document_date");

        builder
            .Property(document => document.SourceCustomer)
            .IsRequired()
            .HasColumnName("source_customer");

        builder
            .Property(document => document.SourceTaskText)
            .IsRequired()
            .HasColumnName("source_task_text");

        builder
            .Property(document => document.SourceIsExternal)
            .HasColumnName("source_is_external");

        builder
            .Property(document => document.SourceOutputDocumentNumber)
            .IsRequired()
            .HasColumnName("source_output_document_number");

        builder
            .Property(document => document.SourceOutputDocumentDate)
            .IsRequired()
            .HasColumnName("source_output_document_date");

        builder
            .Property(document => document.SourceDueDate)
            .IsRequired()
            .HasColumnName("source_due_date");

        builder
            .Property(document => document.SourceResponsibleEmployeeId)
            .IsRequired()
            .HasColumnName("source_responsible_employee_id");

        builder
            .HasOne(document => document.SourceResponsibleEmployee)
            .WithMany()
            .HasForeignKey(document => document.SourceResponsibleEmployeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .Property(document => document.OutputOutgoingNumber)
            .HasColumnName("output_outgoing_number");

        builder
            .Property(document => document.OutputOutgoingDate)
            .HasColumnName("output_outgoing_date");

        builder.Property(document => document.OutputSentTo)
            .HasColumnName("output_sent_to");

        builder
            .Property(document => document.OutputTransferredInWorkOrder)
            .HasColumnName("output_transferred_in_work_order");

        builder
            .Property(document => document.OutputResponseSubmissionMark)
            .HasColumnName("output_response_submission_mark");

        builder
            .Property(document => document.IsUnderControl)
            .HasColumnName("is_under_control");
    }
}