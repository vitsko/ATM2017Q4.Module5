namespace Tests
{
    using Assert;
    using OpenQA.Selenium;
    using Pages;
    using TechTalk.SpecFlow;
    using WDriver;

    [Binding]
    public class BaseTest
    {
        protected static SoftAssertions SoftAssert { get; set; }

        private static IWebDriver Driver { get; set; }

        [BeforeScenario]
        public static void SetupTest()
        {
            Driver = WDriver.Instance;
            WDriver.NavigateTo(Config.StartUrl);
            WDriver.WindowMaximise();
            BaseTest.SoftAssert = new SoftAssertions();
        }

        [AfterScenario]
        public static void CleanUpTestClass()
        {
            BaseTest.SoftAssert.AssertAll();
            SitePages.Close();
            WDriver.Quit();
        }
    }
}