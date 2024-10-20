
using System.Text.Json;
using Fleck;
using lib;
using WS.Models;
using WS.Services;

namespace WS.Events;

public class VoteEvent : BaseEventHandler<VoteEventData>
{
    public override async Task Handle(VoteEventData dto, IWebSocketConnection socket)
    {
        var pool = await Redis.GetData<Poll>(dto.id);

        // The connection should be first and if no one is connected the pool is not in used and shouldn't be on redis
        if (pool == null || pool == default)
            throw new Exception("Pool do not exist");

        var option = pool.Options.FirstOrDefault(e => e.OptionId == dto.option)
            ?? throw new Exception("Do not exist the option");
        option.Votes++;

        await Redis.SetData(dto.id, pool);

        // Broadcast to all users on that room
        StateService.BroadcastToRoom(dto.id, JsonSerializer.Serialize(pool));
    }
}