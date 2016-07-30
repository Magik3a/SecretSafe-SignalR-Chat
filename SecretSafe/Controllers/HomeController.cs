using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models;
using SecretSafe.DataServices;
using SecretSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
namespace SecretSafe.Controllers
{
    public class HomeController : Controller
    {
        private InMemoryRepository _repository;
        private SecretSafeDbContext db = new SecretSafeDbContext();

        private readonly IChatRoomsService chatRoomsService;
        public HomeController(IChatRoomsService chatRoomsService)
        {
            _repository = InMemoryRepository.GetInstance();

            this.chatRoomsService = chatRoomsService;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                SecretSafeUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                return View("Chat", "_Layout", new UserTest { username = currentUser.NickName, roomname = "fix this" });
            }

            return View();
        }



        [HttpPost]
        public ActionResult Index(string username, string roomname)
        {
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "Username is required");
                return View();
            }

            if (string.IsNullOrEmpty(roomname))
            {
                ModelState.AddModelError("room", "Room name is required");
                return View();
            }
            else
            {
                // if we have an already logged user with the same username, then append a random number to it
                if (_repository.Users.Where(u => u.Username.Equals(username)).ToList().Count > 0)
                {
                    username = _repository.GetRandomizedUsername(username);
                }

                return View("Chat", "_Layout", new UserTest { username = username, roomname = roomname});
            }
        }

        [Authorize]
        public ActionResult Rooms()
        {
            var chatRooms = chatRoomsService.GetChatRoomsForUser(User.Identity.Name).ProjectTo<ChooseRoomsViewModel>().ToList();
            return View(chatRooms);
        }


        // For some stupid reason anonymous object can't be send to the view 
        public class UserTest
        {
            public string username { get; set; }

            public string roomname { get; set; }
        }


        public JsonResult DeleteRoomAjax(Guid id)
        {
            return Json(new { id = id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Prices()
        {
            return View();
        }
    }
}