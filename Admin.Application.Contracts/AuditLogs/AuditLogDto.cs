using System;
using Volo.Abp.Application.Dtos;

namespace Admin.AuditLogs
{
    public class AuditLogDto : EntityDto<Guid>
    {
        public Guid? UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ClientIpAddress { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string BrowserInfo { get; set; } = string.Empty;
        public string HttpMethod { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int HttpStatusCode { get; set; }
        public long ExecutionDuration { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string Exceptions { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

        public string ApplicationName { get; set; } = string.Empty;
    }
}
