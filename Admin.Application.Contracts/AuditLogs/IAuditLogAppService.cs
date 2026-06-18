using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Admin.AuditLogs
{
    public interface IAuditLogAppService : IApplicationService
    {
        /// <summary>分页查询审计日志（前端列表）</summary>
        Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListInput input, CancellationToken cancellationToken = default);

        /// <summary>单条日志详情（含完整请求参数、异常堆栈）</summary>
        Task<AuditLogDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
