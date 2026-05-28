using MassTransit;
using Mediator;
using Nimble.Modulith.Email;
using Nimble.Modulith.Email.Web;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting Email microservice web host");

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (Aspire configuration)
builder.AddServiceDefaults();

builder.Services.AddMediator(); 
// Add Mediator with source generation
// use assembly:
// [assembly: Mediator.MediatorOptions(ServiceLifetime = ServiceLifetime.Scoped)]
//builder.Services.AddMediator(options =>
//{
//    options.ServiceLifetime = ServiceLifetime.Scoped;
//}, typeof(EmailModuleExtensions).Assembly);

// Add logging behavior to Mediator pipeline
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Add consumers from the Email module assembly
    x.AddConsumers(typeof(EmailModuleExtensions).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
        cfg.ConfigureEndpoints(context);
    });
});
//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization();

// Add FastEndpoints with JWT Bearer Authentication and Authorization
//builder.Services.AddFastEndpoints()
//    .AddAuthenticationJwtBearer(s =>
//    {
//        s.SigningKey = builder.Configuration["Auth:JwtSecret"];
//    })
//    .AddAuthorization()
//    .SwaggerDocument();

// Add Email module services
builder.AddEmailModuleServices(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add authentication and authorization middleware
//app.UseAuthentication();
//app.UseAuthorization();

//app.UseFastEndpoints()
//    .UseSwaggerGen();

app.Run();