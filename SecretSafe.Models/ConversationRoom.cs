using System.Collections.Generic;

namespace SecretSafe.Models
{
    public class ConversationRoom
    {
        public string RoomName { get; set; }
        public virtual ICollection<ChatUser> Users { get; set; }
    }
}