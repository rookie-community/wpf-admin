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
            builder.ToTable(AdminStoreConsts.DbTablePrefix + "Users", AdminStoreConsts.DbSchema, e => e.HasComment("用户"));
            builder.ConfigureByConvention();
        }
    }
}
