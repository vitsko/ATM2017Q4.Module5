namespace TestPressReleases.Tests
{
    using NUnit.Framework;
    using OpenQA.Selenium.Chrome;
    using WebDriver;

    [TestFixture]
    internal class BaseTest
    {
        protected static ChromeDriver driver = WebDriver.Instance;

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
            WebDriver.Quit();
        }
    }
}