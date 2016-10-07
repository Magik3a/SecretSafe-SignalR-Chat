using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        var language = CultureInfo.CurrentCulture;
        HttpResponseMessage response = Task.Run(() => client.GetAsync($"http://localhost:44113/api/Phrases/{phrase}/{language.TwoLetterISOLanguageName}")).Result;

        if (response.IsSuccessStatusCode)
        {
            var task = Json.Decode(Task.Run(() => response.Content.ReadAsStringAsync()).Result);
            return task.PhraseText;
        }
        else
        {
            return response.StatusCode.ToString();
        }

    }
}