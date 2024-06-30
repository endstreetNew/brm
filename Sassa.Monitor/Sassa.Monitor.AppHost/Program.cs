var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Sassa_Monitor_ApiService>("apiservice");

builder.AddProject<Projects.Sassa_Monitor_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
