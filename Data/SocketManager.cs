using System.Collections.Concurrent;
using System.Net.WebSockets;
  
namespace aspnetmvc_blog.Data
{
    public class SocketManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        public string AddSocket(WebSocket socket, string userid)
        {
            _sockets.TryAdd(userid, socket);
            return userid;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllSockets()
        {
            return _sockets;
        }
    }
}
