namespace TestPressReleases
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using WebDriver;

    internal static class Helper
    {
        internal static void PostHandlingForDateOfPR(List<string> titlesOfPR, bool isPageOfPR)
        {
            if (!isPageOfPR)
            {
                for (int i = 0; i < titlesOfPR.Count; i += 2)
                {
                    titlesOfPR[i] = Regex.Replace(titlesOfPR[i], " -", string.Empty);
                }
            }

            DateTime parse;

            for (int i = 0; i < titlesOfPR.Count; i += 2)
            {
                DateTime.TryParse(titlesOfPR[i], Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);
                titlesOfPR[i] = parse.ToShortDateString();
            }
        }

        internal static bool CheckLinkOfFile(BaseElement element, string attribute)
        {
            var urls = element.GetValueBySpecialFuncSelector(iwebElement => iwebElement.GetAttribute(attribute));

            WebRequest request;
            HttpWebResponse response;

            var sizes = new List<long>();

            for (int i = 0; i < urls.Count; i++)
            {
                request = WebRequest.Create(urls.ElementAt(i));
                response = (HttpWebResponse)request.GetResponse();

                sizes.Add(response.ContentLength);
            }

            return sizes.TrueForAll(size => size > 0);
        }
    }
}