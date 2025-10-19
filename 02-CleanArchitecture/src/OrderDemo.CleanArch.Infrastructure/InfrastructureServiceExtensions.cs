using OrderDemo.CleanArch.Core.Interfaces;
using OrderDemo.CleanArch.Core.Services;
using OrderDemo.CleanArch.Infrastructure.Data;
using OrderDemo.CleanArch.Infrastructure.Data.Queries;
using OrderDemo.CleanArch.UseCases.Contributors.List;
using OrderDemo.CleanArch.UseCases.Products.List;

namespace OrderDemo.CleanArch.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    // Try to get the Aspire connection string first, fallback to SqliteConnection for local development
    string? connectionString = config.GetConnectionString("AppDb") 
                              ?? config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);

    services.AddScoped<EventDispatchInterceptor>();
    services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

    services.AddDbContext<AppDbContext>((provider, options) =>
    {
      var eventDispatchInterceptor = provider.GetRequiredService<EventDispatchInterceptor>();
      
      // Use SQL Server if AppDb connection is available (Aspire), otherwise use SQLite
      if (config.GetConnectionString("AppDb") != null)
      {
        options.UseSqlServer(connectionString);
      }
      else
      {
        options.UseSqlite(connectionString);
      }
      
      // Suppress pending model changes warning - this occurs when design-time DB (SQLite)
      // differs from runtime DB (SQL Server). The model is correct at runtime.
      options.ConfigureWarnings(warnings => 
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
      
      options.AddInterceptors(eventDispatchInterceptor);
    });

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IListProductsQueryService, ListProductsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
