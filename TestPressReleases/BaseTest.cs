namespace Tests
{
    using Assert;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Pages;
    using Serilog;
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

            Log.Information(string.Format(Resource.OpenPage, WDriver.GetDriver().Url));

            WDriver.WindowMaximise();
            this.SoftAssert = new SoftAssertions();
        }

        [TearDown]
        public void CleanUpTestClass()
        {
            SitePages.Close();
            this.SoftAssert.AssertAll();
        }

        protected void WriteToLogFailedSoftAssertsMessage(string message)
        {
            Log.Error(string.Format(Resource.ResultError, message));
        }
    }
}