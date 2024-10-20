
using System.Text.Json;
using Fleck;
using lib;
using Microsoft.EntityFrameworkCore;
using WS.DB;
using WS.Models;
using WS.Services;

namespace WS.Events;

public class ConnectToPollEvent(IServiceProvider serviceProvider) : BaseEventHandler<ConnectEventData>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override async Task Handle(ConnectEventData dto, IWebSocketConnection socket)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MySqliteContext>();

        //Check if the poll exist
        var poll = await dbContext.Polls.Include(e => e.Options).FirstOrDefaultAsync(e => e.PollId == dto.id)
            ?? throw new Exception("There is no pool with this id");

        // Check if is already on use
        if (Redis.Connection.GetDatabase().KeyExists(poll.PollId))
            poll = await Redis.GetData<Poll>(poll.PollId)
                ?? throw new Exception("Error while getting the data");

        // Create a room for the users of the poll
        var isAdded = StateService.AddRoom(socket, poll.PollId);

        if (isAdded)
            poll.connectedPears++;
        await Redis.SetData(poll.PollId, poll);
        await socket.Send(JsonSerializer.Serialize(poll));
    }
}