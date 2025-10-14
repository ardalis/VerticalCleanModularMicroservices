using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderDemo.Api.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Use the same connection string that Aspire uses
        // Aspire typically uses: Server=localhost,PORT;User ID=sa;Password=yourStrong(!)Password;TrustServerCertificate=true
        // Check your Aspire dashboard or AppHost configuration for the exact values
        optionsBuilder.UseSqlServer("Server=localhost,1433;User ID=sa;Password=Your$tr0ngP@ss!;TrustServerCertificate=true;Database=OrderDemo");

        return new AppDbContext(optionsBuilder.Options);
    }
}