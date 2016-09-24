using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using PayPal;
using SecretSafe.Models;
using SecretSafe.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SecretSafe.DataServices;

namespace SecretSafe.Controllers
{
    public class PayPalController : Controller
    {
        private readonly ISecurityLevelsService securityLevels;

        public PayPalController(ISecurityLevelsService securityLevels)
        {
            this.securityLevels = securityLevels;
        }
        // GET: PayPal
        public ActionResult CreatePayment(string Total, string SecurityLevelName)
        {
            var viewData = new PayPalViewData();
            var guid = Guid.NewGuid().ToString();
            decimal total = Convert.ToDecimal(Total);

            var paymentInit = new Payment
            {
                intent = "authorize",
                payer = new Payer
                {

                    payment_method = "paypal",
                },

                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        amount = new Amount
                        {
                            currency = "USD",
                            total = string.Format("{0:N2}" , total)
                        },
                        description = SecurityLevelName
                    }

                },
                redirect_urls = new RedirectUrls
                {
                    return_url = Common.Utilities.ToAbsoluteUrl(HttpContext, String.Format("~/paypal/confirmed?id={0}", guid)),
                    cancel_url = Common.Utilities.ToAbsoluteUrl(HttpContext, String.Format("~/paypal/canceled?id={0}", guid)),
                },
            };

            viewData.JsonRequest = JObject.Parse(paymentInit.ConvertToJson()).ToString(Formatting.Indented);

            try
            {
                var accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["ClientID"], ConfigManager.Instance.GetProperties()["ClientSecret"]).GetAccessToken();
                var apiContext = new APIContext(accessToken);
                var createdPayment = paymentInit.Create(apiContext);

                var approvalUrl = createdPayment.links.ToArray().FirstOrDefault(f => f.rel.Contains("approval_url"));

                if (approvalUrl != null)
                {
                    Session.Add(guid, createdPayment.id);

                    return Redirect(approvalUrl.href);
                }

                viewData.JsonResponse = JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented);

                return View("Error", viewData);
            }
            catch (PayPalException ex)
            {
                viewData.ErrorMessage = ex.Message;

                return View("Error", viewData);
            }
        }

        public ActionResult Confirmed(Guid id, string token, string payerId)
        {
            var viewData = new ConfirmedViewData
            {
                Id = id,
                Token = token,
                PayerId = payerId
            };

            var accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["ClientID"], ConfigManager.Instance.GetProperties()["ClientSecret"]).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            var payment = new Payment()
            {
                id = (string)Session[id.ToString()],
            };

            var executedPayment = payment.Execute(apiContext, new PaymentExecution { payer_id = payerId });

            viewData.AuthorizationId = executedPayment.transactions[0].related_resources[0].authorization.id;
            viewData.JsonRequest = JObject.Parse(payment.ConvertToJson()).ToString(Formatting.Indented);
            viewData.JsonResponse = JObject.Parse(executedPayment.ConvertToJson()).ToString(Formatting.Indented);

            return View(viewData);
        }

        public ActionResult Canceled(Guid id, string token, string payerId)
        {
            return View();
        }

        public ActionResult Capture(string authorizationId)
        {
            var viewData = new PayPalViewData();

            try
            {
                var accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["ClientID"], ConfigManager.Instance.GetProperties()["ClientSecret"]).GetAccessToken();
                var apiContext = new APIContext(accessToken);
                var authorization = Authorization.Get(apiContext, authorizationId);

                if (authorization != null)
                {
                    var total = Convert.ToDecimal(authorization.amount.total);

                    var capture = authorization.Capture(apiContext, new Capture
                    {
                        is_final_capture = true,
                        amount = new Amount
                        {
                            currency = "USD",
                            total = (total + (total * .05m)).ToString("f2")
                        },
                    });


                    viewData.JsonResponse = JObject.Parse(capture.ConvertToJson()).ToString(Formatting.Indented);

                    return View("Success", viewData);
                }

                viewData.ErrorMessage = "Could not find previous authorization.";

                return View("Error", viewData);
            }
            catch (PayPalException ex)
            {
                viewData.ErrorMessage = ex.Message;

                return View("Error", viewData);
            }
        }

        public ActionResult Void(string authorizationId)
        {
            var viewData = new PayPalViewData();

            try
            {
                var accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["ClientID"], ConfigManager.Instance.GetProperties()["ClientSecret"]).GetAccessToken();
                var apiContext = new APIContext(accessToken);
                var authorization = Authorization.Get(apiContext, authorizationId);

                if (authorization != null)
                {
                    var voidedAuthorization = authorization.Void(apiContext);

                    viewData.JsonResponse = JObject.Parse(voidedAuthorization.ConvertToJson()).ToString(Formatting.Indented);

                    return View(viewData);
                }

                viewData.ErrorMessage = "Could not find previous authorization.";

                return View("Error", viewData);
            }
            catch (PayPalException ex)
            {
                viewData.ErrorMessage = ex.Message;

                return View("Error", viewData);
            }
        }
    }
}