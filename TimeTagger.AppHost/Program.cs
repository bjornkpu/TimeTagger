var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.TimeTagger_Api>("api")
    .WithExternalHttpEndpoints();

builder.Build().Run();
