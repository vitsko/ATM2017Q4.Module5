namespace TestPressReleases
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using WebDriver;

    internal static class Helper
    {
        internal static void PostHandlingForDateOfPressReleases(List<string> titlesOfPressReleases, bool isPageOfPressRelease)
        {
            if (!isPageOfPressRelease)
            {
                for (int i = 0; i < titlesOfPressReleases.Count; i += 2)
                {
                    titlesOfPressReleases[i] = Regex.Replace(titlesOfPressReleases[i], " -", string.Empty);
                }
            }

            DateTime parse;

            for (int i = 0; i < titlesOfPressReleases.Count; i += 2)
            {
                DateTime.TryParse(titlesOfPressReleases[i], Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);
                titlesOfPressReleases[i] = parse.ToShortDateString();
            }
        }

        internal static long GetContentLengthForElementOfPressReleaseByLink(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                var request = WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();

                return response.ContentLength;
            }
            else
            {
                return 0;
            }
        }

        internal static void JoinStringsInListByPair(List<string> toPair)
        {
            string date;
            string title;

            for (int i = 0; i < toPair.Count; i++)
            {
                date = toPair.ElementAt(0);
                title = toPair.ElementAt(1);

                toPair.RemoveRange(0, 2);

                toPair.Add(string.Join(" ", new string[] { date, title }));
            }
        }

        internal static List<string> GetListWithOnlySomeDeltaOfIndex(List<string> originalList, int deltaIndex)
        {
            List<string> valueToReturn = new List<string>();

            for (int i = 0; i < originalList.Count; i += deltaIndex)
            {
                valueToReturn.Add(originalList.ElementAt(i));
            }

            return valueToReturn;
        }
    }
}