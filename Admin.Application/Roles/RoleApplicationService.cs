using Admin.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Volo.Abp.Application.Dtos;

namespace Admin.Roles
{
    public class RoleApplicationService : AdminAppService, IRoleApplicationService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RoleApplicationService> _logger;

        public RoleApplicationService(IRoleRepository roleRepository, ILogger<RoleApplicationService> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public Task BatchDelete(IList<Guid> roleIds)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public async Task<PagedResultDto<RoleDto>> GetListAsync(GetRoleListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Role.Id);
            }

            var predicate = GetListFilter(input);
            var roles = await _roleRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, predicate);
            var totalCount = await _roleRepository.CountAsync(predicate);
            return new PagedResultDto<RoleDto>(totalCount, ObjectMapper.Map<List<Role>, List<RoleDto>>(roles));
        }

        private Expression<Func<Role, bool>> GetListFilter(GetRoleListDto model)
        {
            var filter = PredicateBuilder.New<Role>(true);
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                filter = filter.And(x => x.Name.Contains(model.Name));
            }

            return filter!;
        }
    }
}
