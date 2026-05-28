using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

// Add RabbitMQ for inter-service messaging
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

// Add SQL Server database for Identity
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume();

var usersDb = sqlServer.AddDatabase("usersdb");
var productsDb = sqlServer.AddDatabase("productsdb");
var customersDb = sqlServer.AddDatabase("customersdb");
var reportingDb = sqlServer.AddDatabase("reportingdb");

// Papercut SMTP container for email testing
var papercut = builder.AddContainer("papercut", "jijiechen/papercut", "latest")
  .WithEndpoint("smtp", e =>
  {
    e.TargetPort = 25;   // container port
    e.Port = 25;         // host port
    e.Protocol = ProtocolType.Tcp;
    e.UriScheme = "smtp";
  })
  .WithEndpoint("ui", e =>
  {
    e.TargetPort = 37408;
    e.Port = 37408;
    e.UriScheme = "http";
  });

// Add the Web API project with database and Papercut references
var webapi = builder.AddProject<Projects.Nimble_Modulith_Web>("webapi")
    .WithReference(usersDb)
    .WithReference(productsDb)
    .WithReference(customersDb)
    .WithReference(reportingDb)
    .WithReference(rabbitmq)
    .WithEnvironment("Papercut__Smtp__Url", papercut.GetEndpoint("smtp"))
    .WithEnvironment("Papercut__Ui__Url", papercut.GetEndpoint("ui"))
    .WaitFor(usersDb)
    .WaitFor(productsDb)
    .WaitFor(customersDb)
    .WaitFor(reportingDb)
    .WaitFor(rabbitmq)
    .WaitFor(papercut);

// Add the Email microservice with RabbitMQ and Papercut references
var emailService = builder.AddProject<Projects.Nimble_Modulith_Email_Web>("email-service")
    .WithReference(rabbitmq)
    .WithEnvironment("Email__SmtpServer", "localhost")
    .WithEnvironment("Email__SmtpPort", "25")
    .WithEnvironment("Email__EnableSsl", "false")
    .WaitFor(rabbitmq)
    .WaitFor(papercut);

builder.Build().Run();
