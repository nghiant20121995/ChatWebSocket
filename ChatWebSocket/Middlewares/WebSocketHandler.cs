using ChatWebSocket.Controllers;
using ChatWebSocket.Domain.Entities;
using ChatWebSocket.Domain.Interfaces.Services;
using ChatWebSocket.Domain.RequestModel;
using ChatWebSocket.Helper;
using ChatWebSocketHelper;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace ChatWebSocket.Middlewares
{
    public class WebSocketHandler
    {
        private readonly RequestDelegate _next;
        private IUserService _userService;
        private IRoomService _roomService;
        private IUserRoomService _userRoomService;
        private IMessageService _messageService;

        public WebSocketHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _userService = context.RequestServices.GetRequiredService<IUserService>();
            _roomService = context.RequestServices.GetRequiredService<IRoomService>();
            _userRoomService = context.RequestServices.GetRequiredService<IUserRoomService>();
            _messageService = context.RequestServices.GetRequiredService<IMessageService>();
            string? token = context.Request.Query["token"];

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userClaim = jwtToken.Claims.FirstOrDefault(e => e.Type.Equals("UserId"));
            var userId = userClaim?.Value;
            if (userId == null) throw new Exception("UserId is not found");
            using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

            WebSocketConnectionManager.AddSocket(webSocket, userId);

            var buffer = new byte[1024 * 16];

            while (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("WebSocket closed.");
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
                        WebSocketConnectionManager.RemoveSocket(webSocket, userId);

                    }
                    else
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var msgObj = JsonSerializer.Deserialize<MessageRequest>(message);
                        if (msgObj == null) throw new Exception("Message is null");
                        if (!msgObj.IsGroup)
                        {
                            var receiver = await _userService.GetByIdAsync(msgObj.ReceiverId);
                            if (receiver == null) throw new Exception("Reciever doesn't exist");
                            var roomIds = new List<string> { userId, receiver.Id };
                            roomIds.Sort();
                            var roomId = string.Join('_', roomIds);
                            var existingRoom = WebSocketConnectionManager.GetRoomById(roomId);
                            if (existingRoom == null)
                            {
                                await _roomService.CreateRoomAsync(roomId, $"Direct message room for {userId} and {receiver.Id}");
                                await _userRoomService.AddMemberToRoomAsync(roomId, userId);
                                await _userRoomService.AddMemberToRoomAsync(roomId, receiver.Id);
                                WebSocketConnectionManager.CreateRoomById(roomId);
                            }
                            WebSocketConnectionManager.AddMemberToRoom(roomId, userId);
                            WebSocketConnectionManager.AddMemberToRoom(roomId, receiver.Id);
                            await _messageService.CreateAsync(msgObj, userId, roomId);
                            WebSocketConnectionManager.SendMessageToRoom(roomId, msgObj);
                        }
                        //var groupMember = userService.
                        //lsMsg.Add(message);
                        //var json = JsonSerializer.Serialize(lsMsg);
                        //var responseMessage = Encoding.UTF8.GetBytes(json);

                        //var allClients = WebSocketConnectionManager.GetAllSockets();
                        //foreach (var client in allClients)
                        //{
                        //    if (client.State == WebSocketState.Open)
                        //    {
                        //        await client.SendAsync(
                        //        new ArraySegment<byte>(responseMessage, 0, responseMessage.Length),
                        //        WebSocketMessageType.Text,
                        //        true,
                        //        CancellationToken.None);
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            await _next(context);
        }
    }
}
