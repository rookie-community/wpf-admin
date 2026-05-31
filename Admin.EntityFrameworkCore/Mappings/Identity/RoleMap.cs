using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Admin.EntityFrameworkCore.Mappings.Identity
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "Roles", AdminStoreConsts.DbSchema, e => e.HasComment("角色"));
            builder.ConfigureByConvention();
        }
    }
}
