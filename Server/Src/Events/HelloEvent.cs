

using Fleck;
using lib;
using Microsoft.EntityFrameworkCore;
using WS.DB;
using WS.Models;

namespace WS.Events;

public class HelloEvent(IServiceProvider serviceProvider) : BaseEventHandler<HelloEventData>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider; // Use IServiceProvider to resolve scoped services

    public override async Task Handle(HelloEventData dto, IWebSocketConnection socket)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MySqliteContext>();
        var polls = await dbContext.Polls.Include(p => p.Options).ToListAsync();

        await socket.Send($"Hello {dto.Name}");
    }
}