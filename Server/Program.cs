using System.Reflection;
using System.Text.Json;
using Coravel;
using Fleck;
using lib;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using WS.Models;
using WS.Services;
using WS.Tasks;

namespace WS;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScheduler();
        builder.Services.AddTransient<GeneratedPool>();
        builder.Services.AddTransient<SendPool>();


        string? redisConnection =
            builder.Configuration.GetConnectionString("Redis")
            ?? throw new Exception("The connection string of Redis should be stablish");

        var multiplexer = new List<RedLockMultiplexer>
        {
            ConnectionMultiplexer.Connect(redisConnection)
        };
        var redlockFactory = RedLockFactory.Create(multiplexer);

        builder.Services.AddSingleton<IDistributedLockFactory>(redlockFactory);

        var services = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());
        var app = builder.Build();

        app.Services.UseScheduler(scheduler =>
        {
            scheduler.Schedule<GeneratedPool>().Daily();
            scheduler.Schedule<SendPool>().EverySeconds(2);
        });

        string? webSocketConnection =
            builder.Configuration.GetConnectionString("WebSocket")
            ?? throw new Exception("The connection string of WebSocket should be stablish");

        var server = new WebSocketServer(webSocketConnection);

        Redis.Initialize(redlockFactory, redisConnection);
        var db = Redis.Connection.GetDatabase();

        var dailyPoolDb = await Redis.GetData<Pool>("DailyPool");

        if (dailyPoolDb == null || dailyPoolDb == default)
            await Redis.SetData(
                "DailyPool",
                new Pool()
                {
                    Text = "Que prefires?",
                    Options =
                    [
                        new Option() { Id = 1, Text = "Esto" },
                        new Option() { Id = 2, Text = "Nah esto" }
                    ]
                }
            );


        server.Start(socket =>
        {
            socket.OnOpen = async () =>
            {
                StateService.AddConnection(socket);
                Console.WriteLine($"Connected Socket with id {socket.ConnectionInfo.Id}");
                Console.WriteLine($@"Daily pool {Redis.GetRawData("DailyPool")}");
                await socket.Send(Redis.GetRawData("DailyPool"));
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

        app.Run();
    }
}
