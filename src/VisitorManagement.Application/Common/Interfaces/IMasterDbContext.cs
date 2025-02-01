using Microsoft.EntityFrameworkCore;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Common.Interfaces;

public interface IMasterDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
} 