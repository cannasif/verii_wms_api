using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public abstract class BaseLineEntityConfiguration<T> : BaseEntityConfiguration<T> where T : BaseLineEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.StockCode).HasMaxLength(35).IsRequired();
            builder.Property(x => x.Quantity).HasColumnType("decimal(18,6)").IsRequired();
            builder.Property(x => x.Unit).HasMaxLength(10);
            builder.Property(x => x.ErpOrderNo).HasMaxLength(50);
            builder.Property(x => x.ErpOrderId).HasMaxLength(30);
            builder.Property(x => x.Description).HasMaxLength(100);
        }
    }
}
