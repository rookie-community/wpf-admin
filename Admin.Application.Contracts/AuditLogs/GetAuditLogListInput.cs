using System;
using Volo.Abp.Application.Dtos;

namespace Admin.AuditLogs
{
    public class GetAuditLogListInput : PagedAndSortedResultRequestDto
    {
        public string? UserName { get; set; }
        public string? ClientIp { get; set; }
        public string? Url { get; set; }
        public int? HttpStatusCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? HasException { get; set; }
    }
}
