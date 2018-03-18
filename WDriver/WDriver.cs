namespace WDriver
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;

    public class WDriver
    {
        private static RemoteWebDriver driver;
        private static WDriverType currentBrowser;
        private static string browser;

        private WDriver()
        {
        }

        public static IWebDriver Instance
        {
            get
            {
                if (WDriver.driver == null)
                {
                    WDriver.InitDriver();
                    WDriver.driver = WDriveFactory.InitBrowser(currentBrowser);
                }

                return WDriver.driver;
            }
        }

        public static Screenshot ScreenShot { get; private set; }

        public static void WindowMaximise()
        {
            WDriver.driver.Manage().Window.Maximize();
        }

        public static void NavigateTo(string url)
        {
            WDriver.driver.Navigate().GoToUrl(url);
        }

        public static RemoteWebDriver GetDriver()
        {
            return WDriver.driver;
        }

        public static void Quit()
        {
            WDriver.driver.Quit();
            WDriver.driver = null;
        }

        public static void WaitForIsVisible(By by)
        {
            try
            {
                new WebDriverWait(WDriver.GetDriver(), TimeSpan.FromSeconds(int.Parse(Config.Timeout))).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
            }
            catch
            {
                Debug.WriteLine(string.Format("Timed out after {0} seconds)", Config.Timeout));
            }
        }

        public static void HoverOnElement(BaseElement element)
        {
            var elementMoveTo = element.FindElement(element.Locator);
            WDriver.SetBackgroundColorForElement(elementMoveTo);
            new Actions(WDriver.GetDriver())
                                              .MoveToElement(elementMoveTo)
                                              .Build()
                                              .Perform();
        }

        public static IWebDriver OpenLinkInNewTab(IWebElement element)
        {
            ((IJavaScriptExecutor)WDriver.GetDriver())
                                                      .ExecuteScript(string
                                                                           .Format("window.open('{0}'),'_blank'", element.GetAttribute("href")));

            var windowsHandles = WDriver.GetDriver().WindowHandles.ToList();

            return WDriver.GetDriver().SwitchTo().Window(windowsHandles.Last());
        }

        public static void SetValueByScript(string getElementBy, string search, string value)
        {
            WDriver.driver.ExecuteScript(string.Format("document.getElementBy{0}('{1}').setAttribute('value', '{2}')", getElementBy, search, value));
        }

        public static void SetBackgroundColorForElement(IWebElement element)
        {
            WDriver.driver.ExecuteScript("arguments[0].style.backgroundColor = '" + Config.ColorForElement + "'", element);
        }

        public static List<string> GetListValueWithSkipAndTakeElements(By by, int skipCount, int takeCount)
        {
            WDriver.WaitForIsVisible(by);

            return WDriver.driver.FindElements(by)
                                 .ToList()
                                 .Select(iwebElement => iwebElement.Text)
                                 .Skip(skipCount)
                                 .Take(takeCount)
                                 .ToList();
        }

        public static string TakeScreenshot(string directory, string testName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var screenFileName = string.Format(
                                                  "{0} {1}.{2}",
                                                  testName,
                                                  DateTime.Now.ToString("dd.MM hhmmss"),
                                                  ImageFormat.Jpeg.ToString().ToLowerInvariant());

            var screenPass = Path.Combine(directory, screenFileName);

            ScreenShot = ((ITakesScreenshot)WDriver.GetDriver()).GetScreenshot();

            using (var screenshot = Image.FromStream(new MemoryStream(ScreenShot.AsByteArray)))
            {
                screenshot.Save(screenPass);
            }

            return screenPass;
        }

        private static void InitDriver()
        {
            WDriver.browser = Config.Browser;
            Enum.TryParse(WDriver.browser, out currentBrowser);
        }
    }
}