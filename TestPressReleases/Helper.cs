namespace TestPressReleases
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    internal static class Helper
    {
        private static int timeoutInSec = int.Parse(ConfigurationManager.AppSettings["TimeoutInSec"]);

        internal static bool IsExistTextByCSS(string cssSelector)
        {
            return Helper.InnerTextOfElementsFromCssSelector(cssSelector).TrueForAll(item => !string.IsNullOrWhiteSpace(item));
        }

        internal static List<string> InnerTextOfElementsFromCssSelector(string cssSelector)
        {
            var text = new List<string>();

            foreach (var item in Helper.GetAllElementsByCssSelector(cssSelector))
            {
                text.Add(item.Text);
            }

            TestNavigate.CountOfElements = text.Count;

            return text;
        }

        internal static void IsElementVisible(By element)
        {
            new WebDriverWait(TestNavigate.Driver, TimeSpan.FromSeconds(Helper.timeoutInSec)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
        }

        internal static ReadOnlyCollection<IWebElement> GetAllElementsByCssSelector(string cssSelector)
        {
            return TestNavigate.Driver.FindElementsByCssSelector(cssSelector);
        }

        internal static List<string> GetValueOfAttribute(string cssSelector, string attribute)
        {
            var value = new List<string>();

            foreach (var item in Helper.GetAllElementsByCssSelector(cssSelector))
            {
                value.Add(item.GetAttribute(attribute));
            }

            return value;
        }

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
                DateTime.TryParse(titlesOfPR[i], TestNavigate.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);
                titlesOfPR[i] = parse.ToShortDateString();
            }
        }
    }
}
