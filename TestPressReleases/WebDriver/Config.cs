namespace TestPressReleases.WebDriver
{
    using System;
    using System.Configuration;
    using System.Globalization;

    internal static class Config
    {
        private static CultureInfo culture;
        private static DateTime today = DateTime.Today;

        internal static string Timeout => GetEnviromentVar("TimeoutInSec", "2");

        internal static string MenuPressReleases => Config.StartUrl.Contains(".ru") ?
                                                   ConfigurationManager.AppSettings["menuPressReleasesByRus"] :
                                                   ConfigurationManager.AppSettings["menuPressReleasesByEng"];

        internal static string StartUrl => GetEnviromentVar("StartUrl", "http://www.lukoil.ru");

        internal static string URLOfPressReleases => GetEnviromentVar("URLOfPressReleases", "http://www.lukoil.ru/PressCenter/Pressreleases");

        internal static int DefaultCountOfPressReleases => int.Parse(ConfigurationManager.AppSettings["DefaultCountOfPressReleases"]);

        internal static int CountOfPressReleases => int.Parse(ConfigurationManager.AppSettings["DefaultCountOfPressReleases"]);

        internal static int CountOfClickMoreLoad => int.Parse(ConfigurationManager.AppSettings["CountOfClickMoreLoad"]);

        internal static int MaxCountOfPressReleasesOnPage => (Config.CountOfClickMoreLoad + 1) * Config.CountOfPressReleases;

        internal static DateTime LastMonth => Config.today.AddDays(-DateTime.DaysInMonth(Config.today.Year, Config.today.Month));

        internal static string DateFrom => GetEnviromentVar("DateFrom", LastMonth.ToShortDateString());

        internal static string DateTo => GetEnviromentVar("DateTo", Config.today.ToShortDateString());

        internal static CultureInfo Culture
        {
            get
            {
                if (Config.culture == null)
                {
                    Config.culture = Config.StartUrl.Contains(".ru") ? new CultureInfo("ru-RU") : new CultureInfo("en-US");
                }

                return Config.culture;
            }
        }

        internal static bool IsSeleniumGrid => bool.Parse(GetEnviromentVar("IsSeleniumGrid", "false"));

        internal static string URLToHubOfSeleniumGrid => GetEnviromentVar("URLToHubOfSeleniumGrid", "http://localhost:4444/wd/hub");

        internal static string ColorForElement => GetEnviromentVar("ColorForElement", "red");

        private static string GetEnviromentVar(string var, string defaultValue)
        {
            return ConfigurationManager.AppSettings[var] ?? defaultValue;
        }
    }
}