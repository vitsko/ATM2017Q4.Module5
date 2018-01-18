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
                    if (Config.IsSeleniumGrid)
                    {
                        var capability = DesiredCapabilities.Chrome();
                        capability.SetCapability(CapabilityType.BrowserName, "chrome");
                        capability.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                        WebDriver.driver = new RemoteWebDriver(new Uri(Config.URLToHubOfSeleniumGrid), capability);
                    }
                    else
                    {
                        WebDriver.driver = new ChromeDriver();
                    }
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
            var elementMoveTo = element.FindElement(element.Locator);
            WebDriver.SetBackgroundColorForElement(elementMoveTo);
            new Actions(WebDriver.GetDriver())
                                              .MoveToElement(elementMoveTo)
                                              .Build()
                                              .Perform();
        }

        internal static IWebDriver OpenLinkInNewTab(IWebElement element, out List<string> windowsHandles)
        {
            new Actions(WebDriver.GetDriver())
                                              .KeyDown(Keys.Control)
                                              .Build()
                                              .Perform();

            element.Click();

            windowsHandles = WebDriver.GetDriver().WindowHandles.ToList();

            return WebDriver.GetDriver().SwitchTo().Window(windowsHandles.Last());
        }

        internal static void SetValueByScript(string getElementBy, string search, string value)
        {
            WebDriver.driver.ExecuteScript(string.Format("document.getElementBy{0}('{1}').setAttribute('value', '{2}')", getElementBy, search, value));
        }

        internal static void SetBackgroundColorForElement(IWebElement element)
        {
            WebDriver.driver.ExecuteScript("arguments[0].style.backgroundColor = '" + Config.ColorForElement + "'", element);
        }
    }
}