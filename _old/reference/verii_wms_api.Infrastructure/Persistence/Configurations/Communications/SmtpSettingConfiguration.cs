using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SmtpSettingConfiguration : BaseEntityConfiguration<SmtpSetting>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SmtpSetting> builder)
        {
            builder.ToTable("RII_SMTP_SETTING");

            builder.Property(x => x.Host).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(200);
            builder.Property(x => x.FromEmail).HasMaxLength(200);
            builder.Property(x => x.FromName).HasMaxLength(200);
            builder.Property(x => x.PasswordEncrypted).HasMaxLength(2000);

            builder.Property(x => x.Port).IsRequired();
            builder.Property(x => x.Timeout).IsRequired();

            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_SmtpSetting_IsDeleted");
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
