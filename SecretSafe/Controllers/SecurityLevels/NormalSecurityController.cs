using Data;
using Microsoft.AspNet.Identity;
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
        private SecretSafeDbContext db = new SecretSafeDbContext();
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
                string currentUserId = User.Identity.GetUserId();
                var currentUserNickName = db.Users.FirstOrDefault(x => x.Id == currentUserId).NickName;
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
