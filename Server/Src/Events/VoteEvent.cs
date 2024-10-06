
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

        var pool = await Redis.GetData<Pool>("DailyPool");

        if (pool == null || pool == default)
            throw new Exception("Pool do not exist");

        var option = pool.Options.FirstOrDefault(e => e.Id == dto.option)
            ?? throw new Exception("Do not exist the option");
        option.Votes++;

        await Redis.SetData("DailyPool", pool);

        await socket.Send(JsonSerializer.Serialize(pool));
    }
}