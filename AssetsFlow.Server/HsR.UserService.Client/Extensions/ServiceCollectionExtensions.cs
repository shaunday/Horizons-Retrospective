using Grpc.Net.Client;
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace HsR.UserService.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        // Determine the base URL based on environment
        string baseUrl;
        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
        {
            baseUrl = "http://0.0.0.0:80";
        }
        else
        {
            var userServicePort = Environment.GetEnvironmentVariable("USER_SERVICE_PORT") ?? "7001";
            baseUrl = $"https://localhost:{userServicePort}";
        }

        // Register gRPC channel
        services.AddSingleton(serviceProvider =>
        {
            var channel = GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions
            {
                MaxReceiveMessageSize = 1024 * 1024 * 10, // 10MB
                MaxSendMessageSize = 1024 * 1024 * 10,    // 10MB
                HttpHandler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests
                }
            });

            return channel;
        });

        // Register the client
        services.AddSingleton<IUserServiceClient, UserServiceClient>();

        return services;
    }
} 