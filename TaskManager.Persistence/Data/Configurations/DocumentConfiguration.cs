using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Persistence.Data.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder
            .ToTable("documents");

        builder
            .HasKey(document => document.Id);

        builder
            .Property(document => document.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder
            .Property(document => document.SourceOutgoingDocumentNumber)
            .HasColumnName("source_outgoing_document_number")
            .IsRequired();

        builder
            .Property(document => document.SourceOutgoingDocumentDate)
            .HasColumnName("source_outgoing_document_date")
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(document => document.SourceCustomer)
            .HasColumnName("source_customer")
            .IsRequired();

        builder
            .Property(document => document.SourceTaskText)
            .HasColumnName("source_task_text")
            .IsRequired();

        builder
            .Property(document => document.SourceIsExternal)
            .HasColumnName("source_is_external")
            .IsRequired();

        builder
            .Property(document => document.SourceOutputDocumentNumber)
            .HasColumnName("source_output_document_number")
            .IsRequired();

        builder
            .Property(document => document.SourceOutputDocumentDate)
            .HasColumnName("source_output_document_date")
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(document => document.SourceDueDate)
            .HasColumnName("source_due_date")
            .HasColumnType("date")
            .IsRequired();

        builder
            .Property(document => document.IdAuthorCreateDocument)
            .HasColumnName("id_author_create_document")
            .IsRequired();

        builder
            .HasOne(document => document.AuthorCreateDocument)
            .WithMany()
            .HasForeignKey(document => document.IdAuthorCreateDocument)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .Property(document => document.OutputOutgoingNumber)
            .HasColumnName("output_outgoing_number")
            .IsRequired(false);

        builder
            .Property(document => document.OutputOutgoingDate)
            .HasColumnName("output_outgoing_date")
            .HasColumnType("date")
            .IsRequired(false);

        builder
            .Property(document => document.OutputSentTo)
            .HasColumnName("output_sent_to")
            .IsRequired(false);

        builder
            .Property(document => document.OutputTransferredInWorkOrder)
            .HasColumnName("output_transferred_in_work_order")
            .IsRequired(false);

        builder
            .Property(document => document.IsUnderControl)
            .HasColumnName("is_under_control")
            .IsRequired();

        builder
            .Property(document => document.IsCompleted)
            .HasColumnName("is_completed")
            .IsRequired();

        builder
            .Property(document => document.IdAuthorRemoveDocument)
            .HasColumnName("id_author_remove_document")
            .IsRequired(false);

        builder
            .HasOne(document => document.AuthorRemoveDocument)
            .WithMany()
            .HasForeignKey(document => document.IdAuthorRemoveDocument)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder
            .Property(document => document.DateRemove)
            .HasColumnName("date_remove")
            .IsRequired(false);
    }
}