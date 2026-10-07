using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GiftList.Infrastructure.Data;

/// <summary>
/// Used only by `dotnet ef` design-time tooling. At run time Aspire injects the real
/// connection string into the DI container instead, so this factory never runs in production.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=GiftListDesignTime;Trusted_Connection=True;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
