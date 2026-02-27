using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrImportDocumentConfiguration : BaseEntityConfiguration<GrImportDocument>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrImportDocument> builder)
        {
            builder.ToTable("RII_GR_IMPORT_DOCUMENT");
            
            // Properties configuration

            builder.Property(x => x.HeaderId)
                .IsRequired()
                .HasColumnName("HeaderId");

            builder.Property(x => x.Base64)
                .IsRequired()
                .HasColumnName("Base64");

            builder.Property(x => x.ImageUrl)
                .HasColumnName("ImageUrl");

            builder.Property(x => x.FileName)
                .HasColumnName("FileName");

            // Base entity handled by BaseEntityConfiguration

            // Foreign key relationships
            builder.HasOne(x => x.Header)
                .WithMany()
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_GrImportDocument_GrHeader");

            // Indexes
            builder.HasIndex(x => x.HeaderId)
                .HasDatabaseName("IX_GrImportDocument_HeaderId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_GrImportDocument_IsDeleted");

            // Query filter for soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}