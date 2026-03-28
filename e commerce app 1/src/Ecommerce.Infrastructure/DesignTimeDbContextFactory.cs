using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure;

/// <summary>
/// Design-time factory for EF Core migrations.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Data.AppDbContext>
{
    public Data.AppDbContext CreateDbContext(string[] args)
    {
        // Try multiple paths for appsettings - works when running from solution root or project dir
        var possiblePaths = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "src", "Ecommerce.Api"),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "Ecommerce.Api"),
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(DesignTimeDbContextFactory).Assembly.Location)!, "..", "..", "..", "..", "src", "Ecommerce.Api"))
        };

        var basePath = possiblePaths.FirstOrDefault(Directory.Exists) ?? possiblePaths[0];
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connStr = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\mssqllocaldb;Database=EcommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        var optionsBuilder = new DbContextOptionsBuilder<Data.AppDbContext>();
        optionsBuilder.UseSqlServer(connStr);

        return new Data.AppDbContext(optionsBuilder.Options);
    }
}
