

using Coravel.Invocable;
using WS.Services;

namespace WS.Tasks;


public class GeneratedPool : IInvocable
{
    public Task Invoke()
    {
        // TODO every day update the daily pool
        Console.WriteLine("I was invoke");
        return Task.CompletedTask;
    }
}

public class SendPool : IInvocable
{
    public Task Invoke()
    {
        if (StateService.Sockets.Count <= 1)
            return Task.CompletedTask;



        var pool = Redis.GetRawData("DailyPoll");

        if (pool == null)
            return Task.CompletedTask;
        StateService.BroadCastClients(pool);

        return Task.CompletedTask;
    }
}
