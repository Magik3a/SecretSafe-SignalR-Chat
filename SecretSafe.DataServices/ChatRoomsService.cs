namespace SecretSafe.DataServices
{
    using System;
    using Data;
    using SecretSafe.Models;
    using System.Linq;

    public class ChatRoomsService : IChatRoomsService
    {
        private readonly IRepository<ChatRoom> db;

        public ChatRoomsService(IRepository<ChatRoom> db)
        {
            this.db = db;
        }

        public Guid CreateChatRoom(string ChatRoomName, string UserId)
        {
            var chatRoom = new ChatRoom()
            {
                ChatRoomName = ChatRoomName,
                UserId = UserId,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            db.Add(chatRoom);
            db.SaveChanges();

            return chatRoom.Id;
        }

        public void DeleteChatRoom(Guid ChatRoomId)
        {
            var chatRoom = db.GetById(ChatRoomId);
            db.Delete(chatRoom);
            db.SaveChanges();
        }
        public void UpdateChatRoom(Guid ChatRoomId, string ChatRoomName)
        {
            var chatRoom = db.GetById(ChatRoomId);
            chatRoom.ChatRoomName = ChatRoomName;
            db.SaveChanges();
        }

        public ChatRoom GetChatRoomById(Guid ChatRoomId)
        {
            return db.GetById(ChatRoomId);
        }

        public ChatRoom GetChatRoomByName(string ChatRoomName)
        {
            return db.All().Where(c => c.ChatRoomName == ChatRoomName).FirstOrDefault();
        }

        public IQueryable<ChatRoom> GetChatRoomsForUser(string UserId)
        {
            return db.All();
        }

        
    }
}
