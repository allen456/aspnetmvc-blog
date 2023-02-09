using aspnetmvc_blog.Models.Views;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
 
namespace aspnetmvc_blog.Data
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        private SocketManager _manager;
        private const string _defaultname = "guest";
        private const string _publicname = "everyone";
        public SocketMiddleware(RequestDelegate next, SocketManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context, DataContext _dbcontext)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var _user = _defaultname;
                if (context != null && context.User != null && context.User.Identity.IsAuthenticated)
                {
                    _user = context.User.Identity.Name;
                }
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                string conn = _manager.AddSocket(webSocket, _user);
                await SendConnectionID(webSocket, conn);
                await Receive(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        await RouteJSONMessageAsync(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        string id = _manager.GetAllSockets().FirstOrDefault(s => s.Value == webSocket).Key;
                        if (id != null)
                        {
                            _manager.GetAllSockets().TryRemove(id, out WebSocket sock);
                            await sock.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        }
                        return;
                    }
                    else
                    {
                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task RouteJSONMessageAsync(string message)
        {
            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
            if (routeOb.To.ToString() == _publicname)
            {
                foreach (var sock in _manager.GetAllSockets())
                {
                    if (sock.Value.State == WebSocketState.Open)
                    {
                        if (sock.Key.ToString() != _defaultname && routeOb.From.ToString() != _defaultname)
                        {
                            // payload
                            WebSocketsViewModel _payload = new WebSocketsViewModel
                            {
                                PayloadFrom = routeOb.From.ToString(),
                                PayloadTo = sock.Key.ToString(),
                                PayloadData = routeOb.Message.ToString(),
                                PayloadDate = DateTime.Now
                            };
                            await sock.Value.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_payload)), WebSocketMessageType.Text, true, CancellationToken.None);
                            // payload
                        }
                    }
                }
            }
            else
            {
                if (_manager.GetAllSockets().Any(s => s.Key == routeOb.To.ToString()))
                {
                    var sock = _manager.GetAllSockets().FirstOrDefault(s => s.Key == routeOb.To.ToString());
                    var sockreturn = _manager.GetAllSockets().FirstOrDefault(s => s.Key == routeOb.From.ToString());
                    if (sock.Value != null)
                    {
                        if (sock.Value.State == WebSocketState.Open)
                        {
                            if (sock.Key.ToString() != _defaultname && routeOb.From.ToString() != _defaultname)
                            {
                                // payload
                                WebSocketsViewModel _payloadkungwalaproblema = new WebSocketsViewModel
                                {
                                    PayloadFrom = routeOb.From.ToString(),
                                    PayloadTo = routeOb.To.ToString(),
                                    PayloadData = routeOb.Message.ToString(),
                                    PayloadDate = DateTime.Now
                                };
                                await sock.Value.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_payloadkungwalaproblema)), WebSocketMessageType.Text, true, CancellationToken.None);
                                // payload
                            }
                        }
                    }
                }
            }
        }

        private async Task SendConnectionID(WebSocket socket, string connID)
        {
            var buffer = Encoding.UTF8.GetBytes(connID);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
    }
}
