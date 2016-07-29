namespace SecretSafe.DataServices
{
    using Models;
    using System;
    using System.Linq;

    public interface IChatRoomsService
    {
        Guid CreateChatRoom(string ChatRoomName, string UserId);

        void DeleteChatRoom(Guid ChatRoomId);

        void UpdateChatRoom(Guid ChatRoomId, string ChatRoomName);

        IQueryable<ChatRoom> GetChatRoomsForUser(string UserId);

        ChatRoom GetChatRoomByName(string ChatRoomName);

        ChatRoom GetChatRoomById(Guid ChatRoomId);
    }
}