using MultiTenantApp.ServiceTenant.Interface;

namespace MultiTenantApp.ServiceTenant
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetCurrentTenantId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Items.TryGetValue("TenantId", out var tenantId))
            {
                return tenantId as int?;
            }

            return null;
        }
    }
}
