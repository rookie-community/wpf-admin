using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Admin.EntityFrameworkCore.Mappings.Identity
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "UserRole", AdminStoreConsts.DbSchema, e => e.HasComment("用户角色关系表"));
            builder.HasKey(e => new { e.UserId, e.RoleId }); // 在这里指定复合主键
            builder.ConfigureByConvention();
        }
    }
}
