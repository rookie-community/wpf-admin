using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Admin.EntityFrameworkCore.Mappings.Identity
{
    public class PermissionGrantMap : IEntityTypeConfiguration<PermissionGrant>
    {
        public void Configure(EntityTypeBuilder<PermissionGrant> builder)
        {
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "PermissionGrants", AdminStoreConsts.DbSchema, e => e.HasComment("权限授予"));
            builder.ConfigureByConvention();
        }
    }
}
