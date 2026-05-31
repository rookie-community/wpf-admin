using Admin.Localization;
using Volo.Abp.Application.Services;

namespace Admin
{
    public abstract class AdminAppService : ApplicationService
    {
        protected AdminAppService()
        {
            LocalizationResource = typeof(AdminResource);
        }
    }
}
