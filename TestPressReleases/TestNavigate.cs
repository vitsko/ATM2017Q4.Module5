using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using OpenQA.Selenium.Support.UI;

namespace TestPressReleases
{
    [TestFixture]
    public class TestNavigate
    {
        private static ChromeDriver driver = new ChromeDriver();
        private static string listOfReleases = "//div[@class=\"pressrelease-list-widget\"]";
        private static string pressRelease = "//div[@role=\"tablist\"]";

        private static string PressReleasesAfterLoading = "//div[contains(@class,'panel-collapsible')]";

        private static string buttonLoadMore = "//button[@data-bind=\"visible: CanLoadMore\"]";

        private static string cssSelectorAboutPR = string.Format("{0}{1}{2}{3}{4}",
                                                                  "#pressreleaselist-month-",
                                                                  ConfigurationManager.AppSettings["MonthOfPressReleaseWithImage"],
                                                                  " > div:nth-child(",
                                                                  ConfigurationManager.AppSettings["NumberOfPressRealeseInMonth"],
                                                                  ")");

        private static string datePR = string.Format("{0}{1}{2}",
                                                       "#pressreleaselist-month-",
                                                        ConfigurationManager.AppSettings["MonthOfPressReleaseWithImage"],
                                                        " > div:nth-child(2) > div.panel-heading > h2 > span.date");

        private static string titlePR = string.Format("{0}{1}{2}",
                                                       "#pressreleaselist-month-",
                                                        ConfigurationManager.AppSettings["MonthOfPressReleaseWithImage"],
                                                        " > div:nth-child(2) > div.panel-heading > h2 > span.title");

        private static int CountPressReleases = int.Parse(ConfigurationManager.AppSettings["CountPressReleases"]);

        private string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private By listOfReleasesXPhath = By.XPath(listOfReleases);
        private By pressReleaseXPath = By.XPath(pressRelease);

        private By PressReleasesAfterLoadingXPath = By.XPath(PressReleasesAfterLoading);

        private By buttonLoadMoreXPath = By.XPath(buttonLoadMore);

        private int timeoutInSec = int.Parse(ConfigurationManager.AppSettings["TimeoutInSec"]);

        private static IWebElement PressReleaseOnPageByCssSelector
        {
            get
            {
                return driver.FindElementByCssSelector(cssSelectorAboutPR);
            }
        }

        [SetUp]
        public void SetupTest()
        {
            driver.Navigate().GoToUrl(this.baseUrl);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Close();
            driver.Quit();
        }

        [Test]
        public void TestChrome()
        {
            this.BlockIsVisible();
            this.CountOfPressReleases();
            this.CountOfPressReleasesAfterLoading();
            this.AboutPR();
        }

        public void BlockIsVisible()
        {
            this.IsElementVisible(listOfReleasesXPhath);
            Assert.IsTrue(driver.FindElement(listOfReleasesXPhath).Displayed);
        }

        private void CountOfPressReleases()
        {
            Assert.IsTrue(driver.FindElements(pressReleaseXPath).Count == TestNavigate.CountPressReleases);
        }

        private void CountOfPressReleasesAfterLoading()
        {
            driver.FindElement(buttonLoadMoreXPath).Click();
            this.IsElementVisible(buttonLoadMoreXPath);

            Assert.IsTrue(driver.FindElements(PressReleasesAfterLoadingXPath).Count == 2 * TestNavigate.CountPressReleases);
        }

        private void AboutPR()
        {
            this.CheckTitlePR();



        }

        private void CheckTitlePR()
        {
            var date = InnerTextOfElementFromCssSelector(By.CssSelector(TestNavigate.datePR));
            var title = InnerTextOfElementFromCssSelector(By.CssSelector(TestNavigate.titlePR));

            var text = string.Format("{0}{1}", date, title);

            Assert.AreEqual(Resource.TitlePR, text);
        }

        private void CheckImagePR()
        {

        }

        private string InnerTextOfElementFromCssSelector(By by)
        {
            return PressReleaseOnPageByCssSelector.FindElement(by).Text;
        }

        private void IsElementVisible(By element)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(this.timeoutInSec)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
        }



    }


}
