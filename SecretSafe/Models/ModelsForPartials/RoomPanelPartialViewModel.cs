
namespace SecretSafe.Models
{
    using SecretSafe.Infrastructure.Mapping;
    using System;
    using AutoMapper;
    using DataServices;

    public class RoomPanelPartialViewModel : IMapFrom<ChatRoom>, ICustomMapping
    {
        public Guid RoomId { get; set; }

        public string SecurityLevelClass { get; set; }

        public string ChatRoomName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int SecurityLevelId { get; set; }

        public string UserId { get; set; }

        public void CreateMappings(IConfiguration config)
        {
            config.CreateMap<RoomPanelPartialViewModel, ChatRoom>("RoomPanelPartialViewModel")
                .ForMember(c => c.SecurityLevelId, opt => opt.UseValue(SecurityLevelId));

            config.CreateMap<RoomPanelPartialViewModel, ChatRoom>("RoomPanelPartialViewModel")
                .ForMember(c => c.Id, opt => opt.UseValue(RoomId));

            config.CreateMap<RoomPanelPartialViewModel, ChatRoom>("RoomPanelPartialViewModel")
            .ForMember(c => c.CreatedOn, opt => opt.UseValue(CreatedOn));

            config.CreateMap<RoomPanelPartialViewModel, ChatRoom>("RoomPanelPartialViewModel")
            .ForMember(c => c.ModifiedOn, opt => opt.UseValue(DateTime.Now));
        }

    
    }
}