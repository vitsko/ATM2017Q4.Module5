namespace TestPressReleases.Tests
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Pages;
    using WebDriver;

    [TestFixture]
    internal class BaseTest
    {
        protected static IWebDriver driver = WebDriver.Instance;

        [SetUp]
        public void SetupTest()
        {
            driver = WebDriver.Instance;
            WebDriver.NavigateTo(Config.StartUrl);
            WebDriver.WindowMaximise();
        }

        [TearDown]
        public void CleanUpTestClass()
        {
            SitePages.IsOpen = false;
            WebDriver.Quit();
        }
    }
}