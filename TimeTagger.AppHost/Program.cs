var builder = DistributedApplication.CreateBuilder(args);

var dbPassword = builder.AddParameter("postgresql-password");

var postgresDb = builder.AddPostgres("postgres", password: dbPassword, port: 5432)
    .WithDataVolume()
    .AddDatabase("events");

var api = builder.AddProject<Projects.TimeTagger_Api>("api")
    .WithReference(postgresDb)
    .WithExternalHttpEndpoints();

builder.Build().Run();
