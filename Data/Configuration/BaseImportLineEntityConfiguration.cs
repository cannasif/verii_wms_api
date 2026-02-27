using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public abstract class BaseImportLineEntityConfiguration<T> : BaseEntityConfiguration<T> where T : BaseImportLineEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.StockCode).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description1).HasMaxLength(100);
            builder.Property(x => x.Description2).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(255);
        }
    }
}
