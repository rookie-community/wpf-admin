using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging;
using Volo.Abp.Domain.Repositories;

namespace Admin.AuditLogs
{
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogAppService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public Task<AuditLogDto> GetDetailAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListInput input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(AuditLog.ExecutionTime);
            }

            var predicate = GetListFilter(input);
            var authors = await _auditLogRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);//, predicate);
            var totalCount = await _auditLogRepository.CountAsync(predicate);
            return new PagedResultDto<AuditLogDto>(totalCount, ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(authors));
        }

        private Expression<Func<AuditLog, bool>> GetListFilter(GetAuditLogListInput model)
        {
            var filter = PredicateBuilder.New<AuditLog>(true);
            if (!string.IsNullOrWhiteSpace(model.Url))
            {
                filter = filter.And(x => x.HttpMethod.Equals(model.Url));
            }
            return filter;
        }
    }
}
