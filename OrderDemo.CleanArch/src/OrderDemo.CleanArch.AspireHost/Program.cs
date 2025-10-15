var builder = DistributedApplication.CreateBuilder(args);

// Use a random port for the web project
builder.AddProject<Projects.OrderDemo_CleanArch_Web>("web");

builder
  .Build()
  .Run();
