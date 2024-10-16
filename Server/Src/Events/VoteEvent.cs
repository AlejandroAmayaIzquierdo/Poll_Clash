
using System.Text.Json;
using Fleck;
using lib;
using WS.Models;
using WS.Services;
using WS.Vote;

namespace WS.Events;

public class VoteEvent : BaseEventHandler<VoteEventData>
{
    public override async Task Handle(VoteEventData dto, IWebSocketConnection socket)
    {
        var db = Redis.Connection.GetDatabase();

        var pool = await Redis.GetData<Poll>(dto.id);

        if (pool == null || pool == default)
            throw new Exception("Pool do not exist");

        var option = pool.Options.FirstOrDefault(e => e.OptionId == dto.option)
            ?? throw new Exception("Do not exist the option");
        option.Votes++;

        await Redis.SetData(dto.id, pool);

        StateService.BroadCastClients(JsonSerializer.Serialize(pool));
    }
}