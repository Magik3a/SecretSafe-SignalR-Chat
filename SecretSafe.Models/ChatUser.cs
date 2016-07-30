using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSafe.Models
{
    public class ChatUser
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public string RoomName { get; set; }

        public string Color { get; set; }
        //public virtual ICollection<ConversationRoom> Rooms { get; set; }
        public ICollection<Connection> Connections { get; set; }
     
    }
}
