


using Fleck;
using WS.Models;

namespace WS.Services;
public static class StateService
{
    public static readonly Dictionary<Guid, IWebSocketConnection> Sockets = [];
    private static readonly Dictionary<int, HashSet<Guid>> Rooms = [];


    public static bool AddConnection(IWebSocketConnection webSocket)
    {
        return Sockets.TryAdd(webSocket.ConnectionInfo.Id, webSocket);
    }

    public static bool RemoveConnection(Guid id)
    {
        // var roomContainingUser = Rooms.First(e => e.Value.Contains(id));

        // roomContainingUser
        return Sockets.Remove(id);
    }
    public static void AddRoom(IWebSocketConnection socket, int room)
    {
        if (!Rooms.ContainsKey(room))
            Rooms.Add(room, []);
        Rooms[room].Add(socket.ConnectionInfo.Id);
    }

    public static void BroadcastToRoom(int room, string message)
    {
        bool doesRoomExist = Rooms.TryGetValue(room, out var guids);
        if (doesRoomExist || guids == null)
            throw new Exception("Room does not exist");

        foreach (var guid in guids)
        {
            if (Sockets.TryGetValue(guid, out var ws))
                ws.Send(message);
        }
    }

    public static void BroadCastClients(string data)
    {
        foreach (var item in Sockets)
            item.Value.Send(data);
    }
}