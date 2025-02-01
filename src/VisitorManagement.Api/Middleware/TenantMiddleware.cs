using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Api.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip tenant check for tenant creation endpoint
        if (context.Request.Path.StartsWithSegments("/api/tenants") && 
            context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // Get Tenant ID from header
        if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantId))
        {
            if (Guid.TryParse(tenantId, out var parsedTenantId))
            {
                // Add Tenant ID to HttpContext.Items
                context.Items["TenantId"] = parsedTenantId;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid tenant ID format" });
                return;
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Tenant ID is required" });
            return;
        }

        await _next(context);
    }
} 