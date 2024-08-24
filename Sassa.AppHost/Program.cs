var builder = DistributedApplication.CreateBuilder(args);

var brmApi = builder.AddProject<Projects.Sassa_Brm_Api>("sassa-brm-api");

builder.AddProject<Projects.Sassa_BRM>("sassa-brm").WithReference(brmApi);

builder.Build().Run();
