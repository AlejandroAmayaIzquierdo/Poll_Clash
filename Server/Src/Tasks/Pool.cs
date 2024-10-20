

using System.Text.Json;
using Coravel.Invocable;
using WS.Models;
using WS.Services;

namespace WS.Tasks;

public class BroadCastPoolTask : IInvocable
{
    public async Task Invoke()
    {

        foreach (var room in StateService.Rooms)
        {
            var poll = await Redis.GetData<Poll>(room.Key);

            if (poll == null)
                continue;

            if (poll.connectedPears > 1)
                StateService.BroadcastToRoom(room.Key, JsonSerializer.Serialize(poll));
        }
    }
}
