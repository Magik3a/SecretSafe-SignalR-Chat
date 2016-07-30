namespace SecretSafe.DataServices
{
    using Models;
    using System;
    using System.Linq;

    public interface IChatRoomsService
    {
        Guid CreateChatRoom(ChatRoom ChatRoom);

        void DeleteChatRoom(Guid ChatRoomId);

        void UpdateChatRoom(ChatRoom chatRoom);

        IQueryable<ChatRoom> GetChatRoomsForUser(string UserId);

        IQueryable<ChatRoom> GetChatRoomByName(string ChatRoomName);

        IQueryable<ChatRoom> GetChatRoomById(Guid ChatRoomId);
    }
}