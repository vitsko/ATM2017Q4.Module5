namespace TestPressReleases.Tests
{
    using Assert;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Pages;
    using WebDriver;

    [TestFixture]
    internal class BaseTest
    {
        protected SoftAssertions SoftAssert { get; set; }

        private static IWebDriver Driver { get; set; }

        [SetUp]
        public void SetupTest()
        {
            Driver = WebDriver.Instance;
            WebDriver.NavigateTo(Config.StartUrl);
            WebDriver.WindowMaximise();
            this.SoftAssert = new SoftAssertions();
        }

        [TearDown]
        public void CleanUpTestClass()
        {
            this.SoftAssert.AssertAll();
            SitePages.IsOpen = false;
            WebDriver.Quit();
        }
    }
}