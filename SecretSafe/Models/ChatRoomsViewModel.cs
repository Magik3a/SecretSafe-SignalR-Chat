namespace SecretSafe.Models
{
    using Infrastructure.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChatRoomsViewModel : IMapFrom<ChatRoom>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Chat room name is required")]
        [Display(Name = "Chat Room Name")]
        public string ChatRoomName { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
      
    }
}