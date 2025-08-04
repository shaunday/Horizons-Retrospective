var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AssetsFlowWeb_API>("assetsflowweb-api");

builder.AddProject<Projects.HsR_UserService_Host>("hsr-userservice-host");

builder.Build().Run();
