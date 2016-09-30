using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data;
using SecretSafe.Models;
using SecretSafe.DataServices;
using Microsoft.AspNet.Identity;
using AutoMapper.QueryableExtensions;
using AutoMapper;
namespace SecretSafe.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly IChatRoomsService chatRoomsService;
        private readonly ISecurityLevelsService securityLevelsService;
        
        public ChatRoomsController(IChatRoomsService chatRoomsService, ISecurityLevelsService securityLevelsService)
        {
            this.chatRoomsService = chatRoomsService;
            this.securityLevelsService = securityLevelsService;
            // TODO: This should not be needed, but mapper don't create maps from chatRoomsService to view models
            //Mapper.CreateMap<ChatRoom, ChatRoomsViewModel>();
            //Mapper.CreateMap<ChatRoom, ChatRoomsViewModel>().ReverseMap();

            //Mapper.AssertConfigurationIsValid();
        }
        // GET: ChatRooms
        public ActionResult Index()
        {
           
            var chatRooms = chatRoomsService.GetChatRoomsForUser(User.Identity.Name).ProjectTo<ListedChatRoomsViewModel>().ToList();
            return View(chatRooms);
        }

        // GET: ChatRooms/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chatRoom = chatRoomsService.GetChatRoomById(id).ProjectTo<ChatRoomsViewModel>().FirstOrDefault();
            if (chatRoom == null)
            {
                return HttpNotFound();
            }
            chatRoom.SecurityLevel = securityLevelsService.Get(chatRoom.SecurityLevelId).FirstOrDefault().Name;
            return View(chatRoom);
        }

        // GET: ChatRooms/Create
        public ActionResult Create()
        {
            ViewBag.SecurityLevels = new SelectList(securityLevelsService.GetAll(), "SecurityLevelId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChatRoomsViewModel chatRoomViewModel)
        {
            var listSecurityLevels = securityLevelsService.GetAll();
            if (ModelState.IsValid)
            {
                chatRoomViewModel.UserId = User.Identity.GetUserId();
                chatRoomsService.CreateChatRoom(Mapper.Map<ChatRoomsViewModel, ChatRoom>(chatRoomViewModel));
                return RedirectToAction("Index");
            }
            ViewBag.SecurityLevels = new SelectList(listSecurityLevels, "SecurityLevelId", "Name");

            return View(chatRoomViewModel);
        }

        // GET: ChatRooms/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chatRoom = chatRoomsService.GetChatRoomById(id).ProjectTo<ChatRoomsViewModel>().FirstOrDefault();
            if (chatRoom == null)
            {
                return HttpNotFound();
            }
            return View(chatRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChatRoomsViewModel chatRoomViewModel)
        {
            if (ModelState.IsValid)
            {
                chatRoomsService.UpdateChatRoom(Mapper.Map<ChatRoomsViewModel, ChatRoom>(chatRoomViewModel));
                return RedirectToAction("Index");
            }
            return View(chatRoomViewModel);
        }

        // GET: ChatRooms/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chatRoom = chatRoomsService.GetChatRoomById(id).ProjectTo<ChatRoomsViewModel>().FirstOrDefault();

            if (chatRoom == null)
            {
                return HttpNotFound();
            }

            return View(chatRoom);
        }

        // POST: ChatRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            chatRoomsService.DeleteChatRoom(id);
            return RedirectToAction("Index");
        }
        
    }
}
