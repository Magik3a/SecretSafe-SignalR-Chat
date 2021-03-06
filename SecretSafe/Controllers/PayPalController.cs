﻿using System;
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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Data;
using AutoMapper;
using System.Web.Security;
using System.Security.Claims;
using Microsoft.Owin;
using System.Globalization;

namespace SecretSafe.Controllers
{
    public class PayPalController : BaseController<SecretSafeDbContext>
    {
        private readonly ISecurityLevelsService securityLevels;
        private readonly IPaymentsService paymentsService;
        private UserManager _userManager;

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public PayPalController(
            ISecurityLevelsService securityLevels,
            IPaymentsService paymentsService,
            UserManager userManager) : base(new SecretSafeDbContext())
        {
            this.securityLevels = securityLevels;
            this.paymentsService = paymentsService;
            UserManager = userManager;
        }
        // GET: PayPal
        public ActionResult CreatePayment(string Total, string SecurityLevelName)
        {
            var viewData = new PayPalViewData();
            var guid = Guid.NewGuid().ToString();
            decimal total = Convert.ToDecimal(Total, CultureInfo.InvariantCulture);

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
                    Session.Add(guid, new PaymentDescription { Id = createdPayment.id, SecurityLevelName = SecurityLevelName });

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

            var session = (PaymentDescription)Session[id.ToString()];
            var payment = new Payment()
            {
                id = session.Id
            };

            var executedPayment = payment.Execute(apiContext, new PaymentExecution { payer_id = payerId });

            viewData.AuthorizationId = executedPayment.transactions[0].related_resources[0].authorization.id;
            viewData.JsonRequest = JObject.Parse(payment.ConvertToJson()).ToString(Formatting.Indented);
            viewData.JsonResponse = JObject.Parse(executedPayment.ConvertToJson()).ToString(Formatting.Indented);
            viewData.SecurityLevelName = session.SecurityLevelName;
            return View(viewData);
        }

        public ActionResult Canceled(Guid id, string token, string payerId)
        {
            return View();
        }

        public ActionResult Capture(string authorizationId, string securityLevelName)
        {
            var viewData = new PayPalViewData();

            try
            {
                var accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["ClientID"], ConfigManager.Instance.GetProperties()["ClientSecret"]).GetAccessToken();
                var apiContext = new APIContext(accessToken);
                if (authorizationId == null)
                {
                    viewData.ErrorMessage = "Could not find previous authorization.";

                    return View("Error", viewData);
                }
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
                    if (capture.state == "completed")
                    {
                        var userId = User.Identity.GetUserId();
                        var currentRole = UserManager.GetRoles(userId);

                        var payment = new PaymentViewModel()
                        {
                            BeforeRole = currentRole[0],
                            PaymentRole = securityLevelName,
                            DateCreated = DateTime.Now,
                            PaymentNumber = capture.parent_payment,
                            Total = Decimal.Parse(capture.amount.total),
                            UserId = User.Identity.GetUserId(),
                            ExpirationDate = DateTime.Now.AddMonths(1)
                        };

                        // Save the completed payment in database
                        paymentsService.CreatePayment(Mapper.Map<PaymentViewModel, UserPayments>(payment));

                        ChangeUserRoleAfterPayment(securityLevelName);

                    }
                    viewData.SecurityLevelName = securityLevelName;
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



        private bool ChangeUserRoleAfterPayment(string securityLevelName)
        {
            var userID = User.Identity.GetUserId();
            var currentRole = UserManager.GetRoles(userID);

            // Remove User from current role and add the new one if is different
            if (currentRole[0] != securityLevelName)
            {
                UserManager.RemoveFromRole(userID, currentRole[0]);
                UserManager.AddToRole(userID, securityLevelName);

                // TODO Count remaining days and add them to the new role
                User.Identity.SetNewExpirationDateForCurrentRole(DateTime.Now.AddMonths(1));

            }
            else
            {
                var currentExpirationDate = User.Identity.GetExpirationDateForCurrentRole();
                User.Identity.SetNewExpirationDateForCurrentRole(currentExpirationDate.AddMonths(1));
            }

            var user = UserManager.FindById(userID);
            user.ExpirationDateForCurrentRole = User.Identity.GetExpirationDateForCurrentRole();
            IdentityResult result = UserManager.Update(user);

            return true;
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

        private class PaymentDescription
        {
            public string Id { get; set; }

            public string SecurityLevelName { get; set; }
        }
    }
}