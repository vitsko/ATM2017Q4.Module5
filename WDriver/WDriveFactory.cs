namespace WDriver
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Remote;

    internal class WDriveFactory
    {
        private static RemoteWebDriver driver;

        public static RemoteWebDriver InitBrowser(WDriverType browserType)
        {
            switch (browserType)
            {
                case WDriverType.Chrome:
                default:
                    SetOptionsAndDriver(new ChromeOptions(), new ChromeDriver());
                    break;

                case WDriverType.Firefox:
                    SetOptionsAndDriver(new FirefoxOptions(), new FirefoxDriver());
                    break;
            }

            return driver;
        }

        private static void SetOptionsAndDriver(DriverOptions options, RemoteWebDriver instanceOfdriver)
        {
            if (Config.IsSeleniumGrid)
            {
                options.PlatformName = PlatformType.Windows.ToString();
                driver = new RemoteWebDriver(new Uri(Config.URLToHubOfSeleniumGrid), options.ToCapabilities());
            }
            else
            {
                driver = instanceOfdriver;
            }
        }
    }
}