using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Admin.OrganizationUnits
{
    /// <summary>
    /// 组织单元应用服务接口
    /// </summary>
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        /// 获取组织单元列表（树形结构）
        /// </summary>
        Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(GetOrganizationUnitsInput input);

        /// <summary>
        /// 获取单个组织单元详情
        /// </summary>
        Task<OrganizationUnitDto> GetAsync(Guid id);

        /// <summary>
        /// 创建组织单元
        /// </summary>
        Task<OrganizationUnitDto> CreateAsync(CreateOrganizationUnitInput input);

        /// <summary>
        /// 更新组织单元
        /// </summary>
        Task<OrganizationUnitDto> UpdateAsync(UpdateOrganizationUnitInput input);

        /// <summary>
        /// 删除组织单元
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 获取组织单元成员列表
        /// </summary>
        Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(GetOrganizationUnitUsersInput input);

        /// <summary>
        /// 添加成员到组织单元
        /// </summary>
        Task AddUserAsync(Guid organizationUnitId, Guid userId);

        /// <summary>
        /// 从组织单元移除成员
        /// </summary>
        Task RemoveUserAsync(Guid organizationUnitId, Guid userId);

        /// <summary>
        /// 获取组织单元角色列表
        /// </summary>
        Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(GetOrganizationUnitRolesInput input);

        /// <summary>
        /// 添加角色到组织单元
        /// </summary>
        Task AddRoleAsync(Guid organizationUnitId, Guid roleId);

        /// <summary>
        /// 从组织单元移除角色
        /// </summary>
        Task RemoveRoleAsync(Guid organizationUnitId, Guid roleId);
    }
}
