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
                StateService.AddConnection(socket);
                await socket.Send("Connection success");
            };
            socket.OnClose = () =>
            {
                StateService.RemoveConnection(socket.ConnectionInfo.Id);
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


        app.Run();
    }
}
