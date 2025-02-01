using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VisitorManagement.Infrastructure.Data;

public class MasterDbContextFactory : IDesignTimeDbContextFactory<MasterDbContext>
{
    public MasterDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MasterDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=visitor_master;Username=your_username;Password=your_password");

        return new MasterDbContext(optionsBuilder.Options);
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        return new ApplicationDbContext("Host=localhost;Port=5432;Database=visitor_template;Username=your_username;Password=your_password");
    }
} 