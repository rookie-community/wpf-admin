using System;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace Admin.AuditLogs
{
    public class GetAuditLogListInput : PagedAndSortedResultRequestDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string? UserName { get; set; }
        public string? Url { get; set; }

        public int? MinDuration { get; set; }

        public int? MaxDuration { get; set; }

        public string? HttpMethod { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }

        public string? ApplicationName { get; set; }
        public string? ClientIpAddress { get; set; }
    }
}
