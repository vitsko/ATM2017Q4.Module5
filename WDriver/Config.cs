namespace WDriver
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;

    public static class Config
    {
        private static CultureInfo culture;
        private static DateTime today = DateTime.Today;

        public static string Browser => GetEnviromentVar("Browser", "Chrome");

        public static string Timeout => GetEnviromentVar("TimeoutInSec", "2");

        public static string MenuPressReleases => Config.StartUrl.Contains(".ru") ?
                                                   ConfigurationManager.AppSettings["menuPressReleasesByRus"] :
                                                   ConfigurationManager.AppSettings["menuPressReleasesByEng"];

        public static string StartUrl => GetEnviromentVar("StartUrl", "http://www.lukoil.ru");

        public static string URLOfPressReleases => GetEnviromentVar("URLOfPressReleases", "http://www.lukoil.ru/PressCenter/Pressreleases");

        public static int DefaultCountOfPressReleases => int.Parse(ConfigurationManager.AppSettings["DefaultCountOfPressReleases"]);

        public static int CountOfPressReleases => int.Parse(ConfigurationManager.AppSettings["DefaultCountOfPressReleases"]);

        public static int CountOfClickMoreLoad => int.Parse(ConfigurationManager.AppSettings["CountOfClickMoreLoad"]);

        public static int MaxCountOfPressReleasesOnPage => (Config.CountOfClickMoreLoad + 1) * Config.CountOfPressReleases;

        public static DateTime LastMonth => Config.today.AddDays(-DateTime.DaysInMonth(Config.today.Year, Config.today.Month));

        public static string DateFrom => GetEnviromentVar("DateFrom", LastMonth.ToShortDateString());

        public static string DateTo => GetEnviromentVar("DateTo", Config.today.ToShortDateString());

        public static CultureInfo Culture
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

        public static string PatternToUrlOnPageOfPressRelease => string.Format(ConfigurationManager.AppSettings["URLToPageofPressRelease"], LanguageVersion);

        public static bool IsSeleniumGrid => bool.Parse(GetEnviromentVar("IsSeleniumGrid", "false"));

        public static string URLToHubOfSeleniumGrid => GetEnviromentVar("URLToHubOfSeleniumGrid", "http://localhost:4444/wd/hub");

        public static string ColorForElement => GetEnviromentVar("ColorForElement", "red");

        public static string FolderToScreenshot => Path.Combine(".", GetEnviromentVar("FolderToScreenshot", "screenshot"));

        public static string FolderToLog => Path.Combine(".", GetEnviromentVar("FolderToLog", "logs"));

        public static string FileToLog => Path.Combine(Config.FolderToLog, string.Format("{0}-{1}.txt", GetEnviromentVar("FileToLog", "log"), DateTime.Now.ToString("dd.MM")));

        public static string FolderToReport => Path.Combine(".", GetEnviromentVar("FolderToReport", "reports"));

        public static string FileToReport => Path.Combine(Config.FolderToReport, string.Format("{0}-{1}.html", GetEnviromentVar("FileToReport", "report"), DateTime.Now.ToString("dd.MM hhmmss")));

        private static string LanguageVersion => Config.URLOfPressReleases.Contains(".ru") ? "ru" : "en";

        private static string GetEnviromentVar(string var, string defaultValue)
        {
            return ConfigurationManager.AppSettings[var] ?? defaultValue;
        }
    }
}