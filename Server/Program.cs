using System.Reflection;
using System.Text.Json;
using Coravel;
using Fleck;
using lib;
using Microsoft.EntityFrameworkCore;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using WS.DB;
using WS.Services;
using WS.Tasks;

namespace WS;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScheduler();
        builder.Services.AddTransient<BroadCastPoolTask>();


        string? redisConnection =
            builder.Configuration.GetConnectionString("Redis")
            ?? throw new Exception("The connection string of Redis should be stablish");

        var multiplexer = new List<RedLockMultiplexer>
        {
            ConnectionMultiplexer.Connect(redisConnection)
        };
        var redlockFactory = RedLockFactory.Create(multiplexer);

        builder.Services.AddSingleton<IDistributedLockFactory>(redlockFactory);

        var allowedHosts =
            builder
                .Configuration.GetValue<string>("AllowedHosts")
                ?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (allowedHosts.Contains("*"))
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                else
                    policy.WithOrigins(allowedHosts).AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddRegisterRoutes();

        string? connectionString = builder.Configuration.GetConnectionString("Mysql");

        if (connectionString != null)
            builder.Services.AddDbContext<MySqliteContext>(options => options
                .UseMySQL(connectionString));

        var services = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

        var app = builder.Build();


        app.Services.UseScheduler(scheduler =>
        {
            scheduler.Schedule<BroadCastPoolTask>().EverySeconds(2);
        });

        string? webSocketConnection =
            builder.Configuration.GetConnectionString("WebSocket")
            ?? throw new Exception("The connection string of WebSocket should be stablish");


        var server = new WebSocketServer(webSocketConnection);
        Redis.Initialize(redlockFactory, redisConnection);

        server.Start(socket =>
        {
            socket.OnOpen = async () =>
            {
                if (socket.ConnectionInfo.Headers.TryGetValue("Origin", out string? value))
                {
                    var origin = value;

                    // List of allowed origins (e.g., your website domain)
                    var allowedOrigins = allowedHosts;
                    // Check if the Origin is in the allowed origins list
                    if (allowedHosts.Contains("*"))
                    {
                        StateService.AddConnection(socket);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(origin) && allowedOrigins.Contains(origin))
                    {
                        StateService.AddConnection(socket);
                        return;
                    }
                }

                // // If the origin is not allowed, close the connection
                await socket.Send("Unauthorized connection");
                socket.Close();  // Close the WebSocket connection
            };
            socket.OnClose = async () =>
            {
                Console.WriteLine($"Socket {socket.ConnectionInfo.Id} on close");


                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<MySqliteContext>();
                await StateService.RemoveConnection(socket.ConnectionInfo.Id, dbContext);

            };

            socket.OnMessage = async message =>
            {
                try
                {
                    await app.InvokeClientEventHandler(services, socket, message);
                }
                catch (Exception e)
                {
                    var errorObj = new { error = e.Message };
                    var messageError = JsonSerializer.Serialize(errorObj);
                    await socket.Send(messageError);
                }
            };
        });

        app.UseMiddleware<ResponseWrapperMiddleware>();
        app.UseRegisterRoutes();
        app.UseCors();



        app.Run();
    }
}
