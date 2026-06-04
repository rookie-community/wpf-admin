using Admin.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ConnectionStringName("Default")]
    public sealed class AdminEfCoreDbContext : AbpDbContext<AdminEfCoreDbContext>, ITenantManagementDbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<PermissionGrant> PermissionGrants => Set<PermissionGrant>();

        public DbSet<Tenant> Tenants => Set<Tenant>();

        public DbSet<TenantConnectionString> TenantConnectionStrings => Set<TenantConnectionString>();

        public AdminEfCoreDbContext(DbContextOptions<AdminEfCoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureTenantManagement();

            //实体配置
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // 没有配置选项时配置SQLite内存数据库
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlite(new SqliteConnection("Data Source=file::memory:?cache=shared"));
            }
        }
    }
}
