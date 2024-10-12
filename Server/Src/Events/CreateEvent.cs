using Fleck;
using lib;
using Microsoft.EntityFrameworkCore;
using WS.DB;
using WS.Vote;

namespace WS.Events;

public class CreateEvent : BaseEventHandler<VoteEventData>
{

    public override Task Handle(VoteEventData dto, IWebSocketConnection socket)
    {
        // TODO
        return Task.CompletedTask;
    }
}