var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AssetsFlowWeb_API>("assetsflowweb-api");

builder.Build().Run();
