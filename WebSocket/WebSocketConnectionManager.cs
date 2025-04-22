using System.Net.WebSockets;
public static class WebSocketConnectionManager
{
    private static readonly object lockObj = new object();
    private static Dictionary<string, List<WebSocket>> _sockets = new();

    public static void AddSocket(WebSocket socket, string userId)
    {
        lock (lockObj)
        {
            var webSocketId = Guid.NewGuid().ToString();
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

    //public static WebSocket? GetSocketById(string userId, string id)
    //{
    //    if (_sockets.ContainsKey(userId))
    //    {
    //        return _sockets[userId].FirstOrDefault(e => e.Id)
    //    }
    //    _sockets.TryGetValue(id, out var socket);
    //    return socket;
    //}

    //public static void RemoveSocket(string id)
    //{
    //    _sockets.TryRemove(id, out _);
    //}
}
