using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Admin.EntityFrameworkCore.Mappings.Identity
{
    public class PermissionMap : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "Permission", AdminStoreConsts.DbSchema, e => e.HasComment("权限表"));
            builder.ConfigureByConvention();
            builder.Property(p => p.PermissionType).HasConversion<string>();
        }
    }
}
