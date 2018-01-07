namespace TestPressReleases.WebDriver
{
    using System;
    using System.Configuration;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    internal static class WebDriver
    {
        private static ChromeDriver driver;

        static WebDriver()
        {
            WebDriver.TimeoutForElement = int.Parse(ConfigurationManager.AppSettings["TimeoutInSec"]);
            WebDriver.driver = new ChromeDriver();
        }

        internal static int TimeoutForElement { get; private set; }

        internal static void WindowMaximise()
        {
            WebDriver.driver.Manage().Window.Maximize();
        }

        internal static void NavigateTo(string url)
        {
            WebDriver.driver.Navigate().GoToUrl(url);
        }

        internal static IWebDriver GetDriver()
        {
            return WebDriver.driver;
        }

        internal static void Quit()
        {
            WebDriver.driver.Quit();
        }
    }
}