using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

public static class HtmlHelperExtensions
{

    private static HttpClient client = new HttpClient();

    public static string MultiLanguage(this HtmlHelper htmlHelper, int phrase)
    {
        var language = CultureInfo.CurrentUICulture;
        var url = ConfigurationManager.AppSettings["MultiLanguageApiUrl"];
        HttpResponseMessage response = Task.Run(() => client.GetAsync($"{url}/Initials/{language.TwoLetterISOLanguageName}/Phrase/{phrase}")).Result;

        if (response.IsSuccessStatusCode)
        {
            var task = Json.Decode(Task.Run(() => response.Content.ReadAsStringAsync()).Result);
            return task;
        }
        else
        {
            return response.StatusCode.ToString();
        }

    }
}