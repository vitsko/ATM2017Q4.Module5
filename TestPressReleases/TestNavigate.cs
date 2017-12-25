using System;
using NUnit;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using System.Text;
using System.Threading;

namespace TestPressReleases
{
    [TestFixture]
    public class TestNavigate
    {
        private IWebDriver driver;
        private static string listOfReleases = "//div[@class=\"pressrelease-list-widget\"]";
        private static string pressRelease = "//div[@role=\"tablist\"]";

        private static string lastPressReleases = "(//div[contains(@class,'panel-default')])[24]";

        private static string buttonLoadMore = "//button[@data-bind=\"visible: CanLoadMore\"]";

        private static string cssSelectorAboutPR = string.Format("{0}{1}{2}{3}{4}",
                                                                  "#pressreleaselist-month-",
                                                                  ConfigurationManager.AppSettings["monthOfPressReleaseWithImage"],
                                                                  " > div:nth-child(",
                                                                  ConfigurationManager.AppSettings["numberOfPressRealeseInMonth"],
                                                                  ")");

        private static string titlePR = string.Format("{0}{1}{2}",
                                                       "//div[@id=\"pressreleaselist-month-",
                                                        ConfigurationManager.AppSettings["monthOfPressReleaseWithImage"],
                                                        "]/div[2]/div[1]/h2/span");


        private string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private By listOfReleasesXPhath = By.XPath(listOfReleases);
        private By pressReleaseXPath = By.XPath(pressRelease);

        private By lastPressReleasesXPath = By.XPath(lastPressReleases);

        private By buttonLoadMoreXPath = By.XPath(buttonLoadMore);
        //private By cssSelectorAboutPRXPath = By.XPath(cssSelectorAboutPR);
        private By titlePRXPath = By.XPath(titlePR);

        const int CountPressReleases = 10;

        private int timeoutInSec = int.Parse(ConfigurationManager.AppSettings["timeoutInSec"]);

        [SetUp]
        public void SetupTest()
        {
            switch (TestContext.CurrentContext.Test.Name)
            {
                case "TestChrome":

                    this.driver = new ChromeDriver();
                    break;

                case "TestFireFox":
                    var service = FirefoxDriverService.CreateDefaultService();
                    this.driver = new FirefoxDriver(service);
                    break;

                case "TestIE":
                    this.driver = new InternetExplorerDriver();
                    break;

                default:
                    break;
            }

            this.driver.Navigate().GoToUrl(this.baseUrl);
            this.driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void CleanUp()
        {
            this.driver.Close();
            this.driver.Quit();
        }

        [Test]
        public void TestChrome()
        {
            var chrome = this.driver as ChromeDriver;

            this.BlockIsVisible();
            this.CountOfPressReleases();
            this.CountOfPressReleasesAfterLoading();
            // AboutPR(chrome);

        }

        public void BlockIsVisible()
        {
            this.IsElementVisible(listOfReleasesXPhath);
            Assert.IsTrue(this.driver.FindElement(listOfReleasesXPhath).Displayed);
        }

        private void CountOfPressReleases()
        {
            Assert.IsTrue(this.driver.FindElements(pressReleaseXPath).Count == TestNavigate.CountPressReleases);
        }

        private void CountOfPressReleasesAfterLoading()
        {
            this.driver.FindElement(buttonLoadMoreXPath).Click();
            this.IsElementVisible(lastPressReleasesXPath);

            Assert.IsTrue(this.driver.FindElements(pressReleaseXPath).Count == 2 * TestNavigate.CountPressReleases);
        }

        //private void AboutPR(ChromeDriver chrome)
        //{
        //    var text = chrome.FindElementByCssSelector(cssSelectorAboutPR).FindElements(titlePRXPath).ToString();

        //    string title;

        //    //foreach (var item in collection)
        //    //{

        //    //}

        //}

        private void IsElementVisible(By element, int timeoutSecs = 5)
        {
            new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeoutSecs)).Until(ExpectedConditions.ElementIsVisible(element));
        }



    }


}
