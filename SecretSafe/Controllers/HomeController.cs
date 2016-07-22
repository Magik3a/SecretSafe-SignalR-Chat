using SecretSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecretSafe.Controllers
{
    public class HomeController : Controller
    {
        private InMemoryRepository _repository;

        public HomeController()
        {
            _repository = InMemoryRepository.GetInstance();
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View("Chat", "_Layout", User.Identity.Name);

            return View();
        }



        [HttpPost]
        public ActionResult Index(string username, string room)
        {
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "Username is required");
                return View();
            }

            if (string.IsNullOrEmpty(room))
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

                return View("Chat", "_Layout", username);
            }
        }
    }
}