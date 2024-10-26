using Fleck;
using Microsoft.EntityFrameworkCore;
using WS.DB;
using WS.Models;

namespace WS.Services;
public static class StateService
{
    private static readonly Dictionary<Guid, IWebSocketConnection> _sockets = [];
    private static readonly Dictionary<string, HashSet<Guid>> _rooms = [];

    public static Dictionary<Guid, IWebSocketConnection> Sockets => _sockets;

    public static Dictionary<string, HashSet<Guid>> Rooms => _rooms;

    public static bool AddConnection(IWebSocketConnection webSocket)
    {
        return _sockets.TryAdd(webSocket.ConnectionInfo.Id, webSocket);
    }

    public static async Task<bool> RemoveConnection(Guid id, MySqliteContext? dbContext = null)
    {
        try
        {
            // Find all rooms containing the user
            foreach (var room in _rooms.Where(e => e.Value.Contains(id)).ToList())
            {
                room.Value.Remove(id); // Remove user from room
                if (room.Value.Count == 0)
                {
                    var poll = await Redis.GetAndRemoveData<Poll>(room.Key);
                    if (poll != null && dbContext != null)
                    {
                        // Find the poll in the database
                        var pollInDb = await dbContext.Polls.Include(e => e.Options).FirstOrDefaultAsync(p => p.PollId == poll.PollId);
                        if (pollInDb != null)
                        {
                            pollInDb.Options = poll.Options;
                            Console.WriteLine(dbContext.Entry(pollInDb).State);


                            await dbContext.SaveChangesAsync();
                        }
                    }
                    _rooms.Remove(room.Key); // Remove the room if it's empty
                }
            }


            // roomContainingUser
            return _sockets.Remove(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

    }
    public static bool AddRoom(IWebSocketConnection socket, string room)
    {
        if (!_rooms.ContainsKey(room))
            _rooms.Add(room, []);
        return _rooms[room].Add(socket.ConnectionInfo.Id);
    }

    public static void BroadcastToRoom(string room, string message)
    {
        bool doesRoomExist = _rooms.TryGetValue(room, out var guids);
        if (!doesRoomExist || guids == null)
            throw new Exception("Room does not exist");

        foreach (var guid in guids)
        {
            if (_sockets.TryGetValue(guid, out var ws))
                ws.Send(message);
        }
    }

    public static void BroadCastClients(string data)
    {
        foreach (var item in _sockets)
            item.Value.Send(data);
    }
}