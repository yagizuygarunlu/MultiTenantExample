using System;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Common.Interfaces;

public interface ITenantService
{
    Task<DbContext> GetTenantDbContextAsync(Guid tenantId);
    Task<Tenant> GetTenantAsync(Guid tenantId);
    string? GetConnectionString();
    void SetTenant(string tenant);
    Guid GetCurrentTenant();
} 