namespace SecretSafe.Controllers
{
    using Data;
    using global::Models;
    using Microsoft.AspNet.Identity;
    using SecretSafe.DataServices;
    using SecretSafe.Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    public class ProSecurityController : Controller
    {
        // GET: ProSecurity
        private InMemoryRepository _repository;
        private readonly IChatRoomsService chatRoomsService;
        private IRepository<SecretSafeUser> db;
        public ProSecurityController(IChatRoomsService chatRoomsService, IRepository<SecretSafeUser> db)
        {
            _repository = InMemoryRepository.GetInstance();
            this.chatRoomsService = chatRoomsService;
            this.db = db;
        }
        // GET: MaximumSecurity
        public ActionResult Index(Guid id)
        {
            var room = chatRoomsService.GetChatRoomById(id).FirstOrDefault();
            if (room == null)
                return View("~/Views/Home/RoomNotFound.cshtml");
            var roomname = room.ChatRoomName;
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUserNickName = db.All().FirstOrDefault(x => x.Id == currentUserId).NickName;
                // if we have an already logged user with the same username, then append a random number to it
                if (_repository.Users.Where(u => u.Username.Equals(currentUserNickName)).ToList().Count > 0)
                {
                    currentUserNickName = _repository.GetRandomizedUsername(currentUserNickName);
                }
                return View("~/Views/Home/Chat.cshtml", "_Layout", new UserTest { username = currentUserNickName, roomname = roomname });
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(Guid id, string username)
        {
            var roomname = chatRoomsService.GetChatRoomById(id).FirstOrDefault().ChatRoomName;
            if (_repository.Users.Where(u => u.Username.Equals(username)).ToList().Count > 0)
            {
                username = _repository.GetRandomizedUsername(username);
            }
            return View("~/Views/Home/Chat.cshtml", "_Layout", new UserTest { username = username, roomname = roomname });
        }

        public class UserTest
        {
            public string username { get; set; }

            public string roomname { get; set; }
        }
    }
}