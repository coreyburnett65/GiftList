var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddAzureSqlServer("sql")
    .RunAsContainer();

var db = sql.AddDatabase("giftlistdb");

var api = builder.AddProject<Projects.GiftList_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.AddViteApp("web", "../../client")
    .WithReference(api)
    .WaitFor(api)
    .WithEnvironment("VITE_API_BASE_URL", api.GetEndpoint("http"))
    .WithExternalHttpEndpoints();

builder.Build().Run();
