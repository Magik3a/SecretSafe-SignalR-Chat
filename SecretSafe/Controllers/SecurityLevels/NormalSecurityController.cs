using SecretSafe.DataServices;
using SecretSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecretSafe.Controllers
{
    public class NormalSecurityController : Controller
    {
        private InMemoryRepository _repository;
        private readonly IChatRoomsService chatRoomsService;
        public NormalSecurityController(IChatRoomsService chatRoomsService)
        {
            _repository = InMemoryRepository.GetInstance();
            this.chatRoomsService = chatRoomsService;
        }
        // GET: NormalSecurity
        public ActionResult Index(Guid id)
        {
            var roomname = chatRoomsService.GetChatRoomById(id).FirstOrDefault().ChatRoomName;
            if (User.Identity.IsAuthenticated)
            {
                return View("~/Views/Home/Chat.cshtml", "_Layout", new UserTest { username = User.Identity.Name, roomname = roomname });
            }
            else
            {
                return View();
                //Random rnd = new Random();
                //var username = rnd.Next(1000);
                //return View("~/Views/Home/Chat.cshtml", "_Layout", new UserTest { username = username.ToString(), roomname = roomname });

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