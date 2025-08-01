using Grpc.Net.Client;
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Client.Models;
using HsR.UserService.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace HsR.UserService.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        string baseUrl = UserServiceEx.GetUserServiceUrl();

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
                    KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
                    EnableMultipleHttp2Connections = true
                }
            });

            return channel;
        });

        // Register the client
        services.AddSingleton<IUserServiceClient, UserServiceClient>();

        return services;
    }
} 