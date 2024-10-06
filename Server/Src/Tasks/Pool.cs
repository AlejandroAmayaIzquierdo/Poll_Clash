

using Coravel.Invocable;

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
        // TODO broadcast current pool
        throw new NotImplementedException();
    }
}
