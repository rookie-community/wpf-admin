using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Admin.EntityFrameworkCore.Mappings.Identity
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "User", AdminStoreConsts.DbSchema, e => e.HasComment("用户表"));
            builder.ConfigureByConvention();
        }
    }
}
