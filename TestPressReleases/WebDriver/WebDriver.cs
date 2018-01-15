namespace TestPressReleases.WebDriver
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    internal class WebDriver
    {
        private static ChromeDriver driver;

        private WebDriver()
        {
            WebDriver.driver = new ChromeDriver();
        }

        internal static ChromeDriver Instance => WebDriver.driver ?? (WebDriver.driver = new ChromeDriver());

        internal static void WindowMaximise()
        {
            WebDriver.driver.Manage().Window.Maximize();
        }

        internal static void NavigateTo(string url)
        {
            WebDriver.driver.Navigate().GoToUrl(url);
        }

        internal static ChromeDriver GetDriver()
        {
            return WebDriver.driver;
        }

        internal static void Close()
        {
            WebDriver.driver.Close();
        }

        internal static void Quit()
        {
            WebDriver.driver.Quit();
            WebDriver.driver = null;
        }

        internal static void WaitForIsVisible(By by)
        {
            new WebDriverWait(WebDriver.GetDriver(), TimeSpan.FromSeconds(int.Parse(Config.Timeout))).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
        }

        internal static void HoverOnElement(BaseElement element)
        {
            Actions builder = new Actions(WebDriver.GetDriver());
            builder.MoveToElement(element.FindElement(element.Locator)).Build().Perform();
        }
    }
}