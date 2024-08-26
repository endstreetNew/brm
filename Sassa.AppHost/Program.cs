using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var brmApi = builder.AddProject<Projects.Sassa_Brm_Api>("sassa-brm-api");

builder.AddProject<Projects.Sassa_BRM>("sassa-brm").WithReference(brmApi);

//builder.AddOracle("brm-db-server").AddDatabase("brm-db","BRMQA");

//public static Aspire.Hosting.ApplicationModel.IResourceBuilder<Aspire.Hosting.ApplicationModel.OracleDatabaseResource> AddDatabase(this Aspire.Hosting.ApplicationModel.IResourceBuilder<Aspire.Hosting.ApplicationModel.OracleDatabaseServerResource> builder, string name, string? databaseName = default);

builder.Build().Run();
