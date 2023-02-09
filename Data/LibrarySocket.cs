using aspnetmvc_blog.Models.Views;
using aspnetmvc_blog.Models;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
namespace aspnetmvc_blog.Data
{
    public class LibrarySocket
    {
        public LibrarySocket(DataContext context, SocketManager manager)
        {
            _context = context;
            _manager = manager;
        }
        private readonly DataContext _context;
        private readonly SocketManager _manager;
        private const string _defaultname = "guest";
        private const string _system = "system";

        public bool SendEveryoneWebsocket(string message)
        {
           
            foreach (var _destination in _context.Users.Where(w => w.UserStatus))
            {
                string touser = _destination.UserName;
                if (_manager.GetAllSockets().Any(s => s.Key == touser))
                {
                    var sock = _manager.GetAllSockets().FirstOrDefault(s => s.Key == touser);
                    if (sock.Value != null)
                    {
                        if (sock.Value.State == WebSocketState.Open)
                        {
                            if (sock.Key.ToString() != _defaultname)
                            {
                                var _websocketMessage = new WebSocketsViewModel
                                {
                                    PayloadFrom = _system,
                                    PayloadTo = sock.Key.ToString(),
                                    PayloadData = message,
                                    PayloadDate = DateTime.Now
                                };
                                sock.Value.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_websocketMessage)), WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
