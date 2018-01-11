namespace TestPressReleases.WebDriver
{
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Interactions;

    internal class WebDriver
    {
        private static ChromeDriver driver;

        private WebDriver()
        {
            WebDriver.driver = new ChromeDriver();
        }

        internal static ChromeDriver Instance => driver ?? (WebDriver.driver = new ChromeDriver());

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

        internal static void HoverOnElement(BaseElement element)
        {
            Actions builder = new Actions(WebDriver.GetDriver());
            builder.MoveToElement(element.FindElement(element.Locator)).Build().Perform();
        }
    }
}