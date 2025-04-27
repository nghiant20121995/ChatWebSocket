using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.RequestModel;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
public static class WebSocketConnectionManager
{
    private static readonly object lockObj = new object();
    private static Dictionary<string, List<WebSocket>> _sockets = new();

    private static Dictionary<string, Dictionary<string, List<WebSocket>>> _roomSockets = new();

    public static void AddSocket(WebSocket socket, string userId)
    {
        lock (lockObj)
        {
            if (_sockets.ContainsKey(userId))
            {
                _sockets[userId].Add(socket);
            }
            else
            {
                List<WebSocket> newWebSocketDic = new();
                newWebSocketDic.Add(socket);
                _sockets.Add(userId, newWebSocketDic);
            }
        }
    }

    public static Dictionary<string, List<WebSocket>>? GetRoomById(string roomId)
    {
        lock (lockObj)
        {
            if (_roomSockets.TryGetValue(roomId, out var room))
            {
                return room;
            }
            return null;
        }
    }

    public static Dictionary<string, List<WebSocket>>? CreateRoomById(string roomId)
    {
        lock (lockObj)
        {
            if (!_roomSockets.TryGetValue(roomId, out var room))
            {
                room = new();
                _roomSockets[roomId] = room;
            }
            return room;
        }
    }

    public static bool AddMemberToRoom(string roomId, string userId)
    {
        lock (lockObj)
        {
            if (!_roomSockets.TryGetValue(roomId, out var existingroom)) return false;
            if (!_sockets.TryGetValue(userId, out var userSockets)) return false;
            if (_roomSockets[roomId].TryGetValue(userId, out var existingUserRoom)) return false;
            existingroom.Add(userId, userSockets);
            return true;
        }
    }

    public static void RemoveSocket(WebSocket webSocket, string userId)
    {
        lock (lockObj)
        {
            if (_sockets.ContainsKey(userId))
            {
                _sockets[userId].Remove(webSocket);
                if (_sockets[userId].Count == 0)
                {
                    _sockets.Remove(userId);
                }
            }
        }
    }

    public static void SendMessageToRoom(string roomId, MessageRequest message)
    {
        lock (lockObj)
        {
            var buffer = new byte[1024 * 16];
            var room = _roomSockets[roomId];
            var msgJson = JsonSerializer.Serialize(message);
            var responseMessage = Encoding.UTF8.GetBytes(msgJson);
            foreach (var dicWebSocket in room)
            {
                foreach (var webSocket in dicWebSocket.Value)
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        webSocket.SendAsync(
                                new ArraySegment<byte>(responseMessage, 0, responseMessage.Length),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None).Wait();
                    }
                }
            }
        }
    }
}
