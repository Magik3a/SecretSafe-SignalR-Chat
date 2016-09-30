namespace SecretSafe.Models
{
    using Infrastructure.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;

    public class ListedChatRoomsViewModel : IMapFrom<ChatRoom>, ICustomMapping
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Chat room name is required")]
        [Display(Name = "Chat Room Name")]
        public string ChatRoomName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string SecurityLevel { get; set; }

        public void CreateMappings(IMapperConfigurationExpression config)
        {
            config.CreateMap<ChatRoom, ListedChatRoomsViewModel>()
                 .ForMember(c => c.SecurityLevel, opt => opt.MapFrom(c => c.SecurityLevels.Name));
        }
    }
}