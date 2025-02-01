using Microsoft.EntityFrameworkCore;
using VisitorManagement.Domain.Entities;
using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Infrastructure.Data;

public class MasterDbContext : DbContext, IMasterDbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.Name)
            .IsUnique();
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
} 