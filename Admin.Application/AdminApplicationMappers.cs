using Admin.AuditLogs;
using Riok.Mapperly.Abstractions;
using Volo.Abp.AuditLogging;
using Volo.Abp.Mapperly;

namespace Admin;

/*
 * You can add your own mappings here.
 * [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
 * public partial class AdminApplicationMappers : MapperBase<BookDto, CreateUpdateBookDto>
 * {
 *    public override partial CreateUpdateBookDto Map(BookDto source);
 * 
 *    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
 * }
 */

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AdminApplicationMappers : MapperBase<AuditLog, AuditLogDto>
{
    public override partial AuditLogDto Map(AuditLog source);
    public override partial void Map(AuditLog source, AuditLogDto destination);
}
