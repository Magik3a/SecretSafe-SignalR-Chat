using Microsoft.AspNet.SignalR;
using SecretSafe.Models;
using SecretSafe.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SecretSafe.Hubs
{
    public class ChatHub : Hub
    {
        private InMemoryRepository _repository;

        public ChatHub()
        {
            _repository = InMemoryRepository.GetInstance();
        }


        #region Rooms
        public Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        #endregion

        #region IDisconnect and IConnected event handlers implementation

        /// <summary>
        /// Fired when a client disconnects from the system. The user associated with the client ID gets deleted from the list of currently connected users.
        /// </summary>
        /// <returns></returns>



        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = _repository.GetUserByConnectionId(Context.ConnectionId);
            if (userId != null)
            {
                ChatUser user = _repository.Users.Where(u => u.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    _repository.Remove(user);
                    return Clients.All.leaves(user.Id, user.Username, DateTime.Now);
                }
            }

            return base.OnDisconnected(true);
        }

        #endregion

        #region Chat event handlers
        /// <summary>
        /// Fired when a client press clean history button.
        /// </summary>
        public void CleanHistory()
        {
            Clients.All.cleanHistoryConfirmed();
        }

        /// <summary>
        /// Fired when a client pushes a message to the server.
        /// </summary>
        /// <param name="message"></param>
        public void Send(ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                // Sanitize input
                message.Content = HttpUtility.HtmlEncode(message.Content);
                // Process URLs: Extract any URL and process rich content (e.g. Youtube links)
                HashSet<string> extractedURLs;
                message.Content = TextParser.TransformAndExtractUrls(message.Content, out extractedURLs);
                message.Timestamp = DateTime.Now;

                message.Color = _repository.Users.FirstOrDefault(u => u.Username == message.Username).Color;
                Clients.All.onMessageReceived(message);
            }
        }

        /// <summary>
        /// Fired when a client joins the chat. Here round trip state is available and we can register the user in the list
        /// </summary>
        public void Joined()
        {
            ChatUser user = new ChatUser()
            {
                //Id = Context.ConnectionId,                
                Id = Guid.NewGuid().ToString(),
                Username = Clients.Caller.username,
                RoomName = Clients.Caller.roomname,
                Color = RandomColorGenerator.GetRandomColor()
            };
            _repository.Add(user);
            _repository.AddMapping(Context.ConnectionId, user.Id);

            Clients.All.joins(
                user.Id,
                Clients.Caller.username,
                Clients.Caller.roomname,
                user.Color,
                DateTime.Now
                );
        }

        /// <summary>
        /// Invoked when a client connects. Retrieves the list of all currently connected users
        /// </summary>
        /// <returns></returns>
        public ICollection<ChatUser> GetConnectedUsers()
        {
            return _repository.Users.ToList<ChatUser>();
        }

        #endregion
    }
}