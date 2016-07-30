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
        private readonly IChatRoomsService db;

        public ChatRoomsController(IChatRoomsService db)
        {
            this.db = db;
            // TODO: This should not be needed, but mapper don't create maps from db to view models
            Mapper.CreateMap<ChatRoom, ChatRoomsViewModel>();
        }
        // GET: ChatRooms
        public ActionResult Index()
        {
           
            var chatRooms = db.GetChatRoomsForUser(User.Identity.Name).ProjectTo<ChatRoomsViewModel>().ToList();
            return View(chatRooms);
        }

        // GET: ChatRooms/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chatRoom = db.GetChatRoomById(id).ProjectTo<ChatRoomsViewModel>().FirstOrDefault();
            if (chatRoom == null)
            {
                return HttpNotFound();
            }
            return View(chatRoom);
        }

        // GET: ChatRooms/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChatRoomsViewModel chatRoomViewModel)
        {
            if (ModelState.IsValid)
            {
                chatRoomViewModel.UserId = User.Identity.GetUserId();
                db.CreateChatRoom(Mapper.Map<ChatRoomsViewModel, ChatRoom>(chatRoomViewModel));
                return RedirectToAction("Index");
            }

            return View(chatRoomViewModel);
        }

        // GET: ChatRooms/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chatRoom = db.GetChatRoomById(id).ProjectTo<ChatRoomsViewModel>().FirstOrDefault();
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
                db.UpdateChatRoom(Mapper.Map<ChatRoomsViewModel, ChatRoom>(chatRoomViewModel));
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
            var chatRoom = db.GetChatRoomById(id).ProjectTo<ChatRoomsViewModel>().FirstOrDefault();
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
            db.DeleteChatRoom(id);
            return RedirectToAction("Index");
        }
        
    }
}
