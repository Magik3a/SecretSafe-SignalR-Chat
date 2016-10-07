using SecretSafe.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecretSafe.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentsService paymentsService;

        public PaymentsController(IPaymentsService paymentsService)
        {
            this.paymentsService = paymentsService;
        }
        // GET: Payments
        public ActionResult Completed()
        {

            return View();
        }
    }
}