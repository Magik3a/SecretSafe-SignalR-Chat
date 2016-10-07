using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace SecretSafe
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        private string _DefaultLanguage = "en";

        public LocalizationAttribute(string defaultLanguage)
        {
            _DefaultLanguage = defaultLanguage;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            string lang = (string)actionContext.RouteData.Values["lang"] ?? _DefaultLanguage;
            if (lang != _DefaultLanguage)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = 
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                }
                catch (Exception ex)
                {

                    throw new NotSupportedException($"ERROR: Invalid language code {lang}");
                }
            }
        }
    }
}