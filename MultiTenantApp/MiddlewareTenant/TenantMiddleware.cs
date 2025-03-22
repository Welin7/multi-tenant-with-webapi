namespace MultiTenantApp.MiddlewareTenant
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("TenantId", out var tenantIdStr)
            && int.TryParse(tenantIdStr, out var tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }

            await _next(context);
        }
    }
}
