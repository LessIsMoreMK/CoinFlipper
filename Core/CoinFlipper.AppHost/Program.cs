var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CoinFlipper_Access_Api>("Access");
builder.AddProject<Projects.CoinFlipper_Notification_Api>("Notification");
builder.AddProject<Projects.CoinFlipper_SwissArmy_Api>("SwissArmy");
builder.AddProject<Projects.CoinFlipper_Tracer_Api>("Tracer");

builder.Build().Run();
