using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Admin.EntityFrameworkCore.Mappings.Identity
{
    public class RolePermissionMap : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "RolePermission", AdminStoreConsts.DbSchema, e => e.HasComment("角色权限关系表"));
            builder.HasKey(e => new { e.RoleId, e.PermissionId }); // 在这里指定复合主键
            builder.ConfigureByConvention();
        }
    }
}
