using System.Text.Json;
using RedLockNet;
using StackExchange.Redis;

namespace WS.Services;

public class Redis
{
    private static IDistributedLockFactory _LockFactory;

    private static Lazy<ConnectionMultiplexer>? _connection;

    public static ConnectionMultiplexer Connection => _connection!.Value;

    public static void Initialize(IDistributedLockFactory lockFactory, string redisConnectionString)
    {
        _LockFactory = lockFactory;

        // Initialize the Redis connection
        _connection = new Lazy<ConnectionMultiplexer>(
            () => ConnectionMultiplexer.Connect(redisConnectionString)
        );
    }

    public static async Task<bool> SetData<T>(string key, T data)
    {
        try
        {
            await using var _lock = await _LockFactory.CreateLockAsync(
                key,
                TimeSpan.FromSeconds(10)
            );

            if (_lock.IsAcquired)
            {
                var db = Connection.GetDatabase();
                db.StringSet(key, JsonSerializer.Serialize(data));
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting data for key {key}: {ex.Message}");
            return false;
        }
    }

    public static async Task<T?> GetData<T>(string key)
    {
        try
        {
            await using var _lock = await _LockFactory.CreateLockAsync(
                key,
                TimeSpan.FromSeconds(10)
            );

            if (_lock.IsAcquired)
            {
                var db = Connection.GetDatabase();
                var res = db.StringGet(key);

                if (!res.HasValue || res.IsNullOrEmpty)
                    return default;

                var obj = JsonSerializer.Deserialize<T>(res.ToString()!);
                return obj;
            }
            return default;
        }
        catch
        {
            return default;
        }
    }

    public static string? GetRawData(string key)
    {
        return Connection.GetDatabase().StringGet(key);
    }
}
