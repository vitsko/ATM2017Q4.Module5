namespace Tests
{
    using Assert;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Pages;
    using WDriver;

    [TestFixture]
    internal class BaseTest : BaseTestForNUnit
    {
        protected SoftAssertions SoftAssert { get; set; }

        private static IWebDriver Driver { get; set; }

        [SetUp]
        public void SetupTest()
        {
            Driver = WDriver.Instance;
            WDriver.NavigateTo(Config.StartUrl);
            WDriver.WindowMaximise();
            this.SoftAssert = new SoftAssertions();
        }

        [TearDown]
        public void CleanUpTestClass()
        {
            SitePages.Close();
            WDriver.Quit();
            this.SoftAssert.AssertAll();
        }
    }
}