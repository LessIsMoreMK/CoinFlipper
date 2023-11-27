var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CoinFlipper_Access_Api>("Access");
builder.AddProject<Projects.CoinFlipper_Notification_Api>("Notification");
builder.AddProject<Projects.CoinFlipper_SwissArmy_Api>("SwissArmy");

builder.AddProject<Projects.CoinFlipper_Tracer_Api>("Tracer");

//.WithServiceBinding(hostPort: 8888, scheme: "http", name: "dashboard");

// var postgres = builder.AddPostgres("postgres")
//     .WithAzureDatabaseForPostgres();
//
// var catalog = builder.AddProject<Projects.CatalogService>()
//     .WithPostgresDatabase(postgres, databaseName: "catalog")
//     .AsHttpService();

builder.Build().Run();
