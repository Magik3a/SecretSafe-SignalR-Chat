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
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Net;

namespace SecretSafe.Controllers
{
    public class HomeController : Controller
    {
        private InMemoryRepository _repository;
        private SecretSafeDbContext db = new SecretSafeDbContext();

        private readonly ISecurityLevelsService securityLevels;
        private readonly IChatRoomsService chatRoomsService;

        public HomeController(IChatRoomsService chatRoomsService, ISecurityLevelsService securityLevels)
        {
            _repository = InMemoryRepository.GetInstance();

            this.chatRoomsService = chatRoomsService;
            this.securityLevels = securityLevels;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUserNickName = db.Users.FirstOrDefault(x => x.Id == currentUserId).NickName;
                ViewBag.UserNickName = currentUserNickName;
            }

            return View();
        }


        public ActionResult TestSPA()
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUserNickName = db.Users.FirstOrDefault(x => x.Id == currentUserId).NickName;
                ViewBag.UserNickName = currentUserNickName;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string roomname)
        {

            if (string.IsNullOrEmpty(username) && !User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("username", "Username is required");
                return View();
            }
            else if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUserNickName = db.Users.FirstOrDefault(x => x.Id == currentUserId).NickName;
                username = currentUserNickName;
            }

            if (string.IsNullOrEmpty(roomname))
            {
                ModelState.AddModelError("room", "Room name is required");
                return View();
            }
            else
            {
                var chatRoomDb = chatRoomsService.GetChatRoomByName(roomname).FirstOrDefault();
                if(chatRoomDb != null)
                {
                    ModelState.AddModelError("room", "Room name is reserved for private user");
                    ViewBag.UserNickName = username;
                    return View();
                }
                // if we have an already logged user with the same username, then append a random number to it
                if (_repository.Users.Where(u => u.Username.Equals(username)).ToList().Count > 0)
                {
                    username = _repository.GetRandomizedUsername(username);
                }

                return View("Chat", "_LayoutTemplate", new UserTest { username = username, roomname = roomname});
            }
        }

        [Authorize]
        public ActionResult Rooms()
        {
            List <ChooseRoomsViewModel> chatRooms = chatRoomsService
                .GetChatRoomsForUser(User.Identity.GetUserId())
                .ProjectTo<ChooseRoomsViewModel>()
                .ToList();
            ViewBag.SecurityLevels = securityLevels.GetAll().ToList();
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
            if (Request.IsAjaxRequest())
            {
                chatRoomsService.DeleteChatRoom(id);
                return Json(new { id = id }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult CreateRoomAjax(string SecurityLevel)
        {


            return PartialView("RoomsPartials/BoxCreateRoomPartial", new CreateRoomPartialModel {  securitylevel = SecurityLevel });
        }

        public ActionResult SaveRoomAjax(string SecurityLevel, string roomname)
        {
            var securityLevelTitle = GetClassSecurityLevel(SecurityLevel);
            int securityLevelId = securityLevels.GetByName(securityLevelTitle).SecurityLevelId;
            string currentUserId = User.Identity.GetUserId();

            var rolesForUser = "";
            using (var userManager = new UserManager<SecretSafeUser>(new UserStore<SecretSafeUser>(new SecretSafeDbContext())))
            {
                rolesForUser = userManager.GetRoles(currentUserId).FirstOrDefault();

            }
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));

            var roleLevel = roleManager.Roles.Where(r => r.Name == rolesForUser).Single().Level;

            if (!CheckUserPermissions(securityLevelTitle, roleLevel))
            {
                return Json(new { status = false, title = securityLevelTitle, cssClass=SecurityLevel }, JsonRequestBehavior.AllowGet);
            }
            var model = new RoomPanelPartialViewModel()
            {
                ChatRoomName = roomname,
                RoomId = new Guid(),
                CreatedOn = DateTime.Now,
                SecurityLevelClass = SecurityLevel,
                SecurityLevelId = securityLevelId,
                UserId = User.Identity.GetUserId()
            };

            model.RoomId = chatRoomsService.CreateChatRoom(Mapper.Map<RoomPanelPartialViewModel, ChatRoom>(model));

            return PartialView("RoomsPartials/RoomPanelPartial", model);
        }



        private bool CheckUserPermissions(string SecurityLevel, int RoleLevel)
        {
            var securityLevel = this.securityLevels.GetByName(SecurityLevel).Level;
            if (RoleLevel < securityLevel)
                return false;
            else
                return true;
        }


        public string GetClassSecurityLevel(string securityLevelTitle)
        {
            switch (securityLevelTitle)
            {
                case "info": return "Normal Security";
                case "success": return "Medium Security";
                case "warning": return "Pro Security";
                case "danger": return "Maximum Security";
                default: return "";
            }
        }
        public class CreateRoomPartialModel
        {
            public string securitylevel { get; set; }
        }



        public ActionResult Prices()
        {
            return View();
        }
    }
}