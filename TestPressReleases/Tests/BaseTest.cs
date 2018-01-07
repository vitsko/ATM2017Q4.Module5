namespace TestPressReleases.Tests
{
    using System.Configuration;
    using NUnit.Framework;
    using TestPressReleases.WebDriver;

    [TestFixture]
    internal class BaseTest
    {
        private static string baseUrl = ConfigurationManager.AppSettings["baseUrl"];

        [SetUp]
        public void SetupTest()
        {
            WebDriver.NavigateTo(BaseTest.baseUrl);
            WebDriver.WindowMaximise();
        }

        [TearDown]
        public void CleanUp()
        {
            WebDriver.Quit();
        }
    }
}