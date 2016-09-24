using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SecretSafe.DataServices;
using SecretSafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SecretSafe.Controllers
{
    public class UpdateController : Controller
    {
        private SecretSafeDbContext db = new SecretSafeDbContext();
        private ISecurityLevelsService securityLevels;
        public UpdateController(ISecurityLevelsService securityLevels)
        {
            this.securityLevels = securityLevels;
        }
        // GET: Update
        public ActionResult Index(int SecurityLevel)
        {
            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));

            var securityLevel = roleManager.Roles.Where(r => r.Level == SecurityLevel).Single();
            var price = securityLevels.GetByName(securityLevel.Name).price;
            var model = new UpdateViewModel() { SecurityLevelName = securityLevel.Name, price = String.Format("{0}.00", price) };

            return View(model);
        }

        public async Task<ActionResult> UpdateUserSecurityLevel()
        {
            return View();
        }

    }
}