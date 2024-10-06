

using Fleck;
using lib;
using WS.Models;

namespace WS.Events;

public class HelloEvent : BaseEventHandler<HelloEventData>
{
    public override Task Handle(HelloEventData dto, IWebSocketConnection socket)
    {
        socket.Send($"Hello {dto.Name}");
        return Task.CompletedTask;
    }
}