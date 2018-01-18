namespace TestPressReleases.WebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;

    internal class WebDriver
    {
        private static RemoteWebDriver driver;

        private WebDriver()
        {
            WebDriver.driver = new ChromeDriver();
        }

        internal static IWebDriver Instance
        {
            get
            {
                if (WebDriver.driver == null)
                {
                    var capability = DesiredCapabilities.Chrome();
                    capability.SetCapability(CapabilityType.BrowserName, "chrome");
                    capability.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                    driver = new RemoteWebDriver(new Uri(Config.URLToHubOfSeleniumGrid), capability);
                }

                return WebDriver.driver;
            }
        }

        internal static void WindowMaximise()
        {
            WebDriver.driver.Manage().Window.Maximize();
        }

        internal static void NavigateTo(string url)
        {
            WebDriver.driver.Navigate().GoToUrl(url);
        }

        internal static RemoteWebDriver GetDriver()
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

        internal static IWebDriver OpenLinkInNewTab(IWebElement element, out List<string> windowsHandles)
        {
            WebDriver.GetDriver().Keyboard.PressKey(Keys.Control);

            element.Click();

            windowsHandles = WebDriver.GetDriver().WindowHandles.ToList();

            return WebDriver.GetDriver().SwitchTo().Window(windowsHandles.Last());
        }
    }
}