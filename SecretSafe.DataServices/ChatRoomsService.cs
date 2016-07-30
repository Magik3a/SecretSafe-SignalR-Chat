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

        public Guid CreateChatRoom(ChatRoom ChatRoom)
        {
            ChatRoom.CreatedOn = DateTime.Now;
            ChatRoom.ModifiedOn = DateTime.Now;
            db.Add(ChatRoom);
            db.SaveChanges();

            return ChatRoom.Id;
        }

        public void DeleteChatRoom(Guid ChatRoomId)
        {
            var chatRoom = db.GetById(ChatRoomId);
            db.Delete(chatRoom);
            db.SaveChanges();
        }

        public void UpdateChatRoom(ChatRoom chatRoom)
        {
            chatRoom.ModifiedOn = DateTime.Now;
            db.Update(chatRoom);
            db.SaveChanges();
        }

        public IQueryable<ChatRoom> GetChatRoomById(Guid ChatRoomId)
        {
            return db.All().Where(c => c.Id == ChatRoomId);
        }

        public IQueryable<ChatRoom> GetChatRoomByName(string ChatRoomName)
        {
            return db.All().Where(c => c.ChatRoomName == ChatRoomName);
        }

        public IQueryable<ChatRoom> GetChatRoomsForUser(string UserId)
        {
            return db.All().OrderBy(c => c.SecurityLevelId);
        }
    }
}
