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

        /// <summary>
        /// (//div[@role='tablist'])//div[@class='panel-body']//div[@class='image']//img
        /// </summary>
        private static ChromeDriver driver = new ChromeDriver();
        private static string listOfReleases = "//div[@class='pressrelease-list-widget']";
        private static string pressRelease = "//div[@role='tablist']";

        private static string PressReleasesAfterLoading = "//div[contains(@class,'panel-collapsible')]";

        private static string buttonLoadMore = "//button[@data-bind='visible: CanLoadMore']";

        private static string CssSelectorAboutPR
        {
            get
            {
                return string.Format("#{0}{1}", driver.FindElement(By.XPath("(//a[@class='point point-month'])[1]")).GetAttribute("aria-controls"), " > div:nth-child(1)");
            }
        }

        private static string DatePR
        {
            get
            {
                return string.Format("#{0}{1}", driver.FindElement(By.XPath("(//a[@class='point point-month'])[1]")).GetAttribute("aria-controls"), " > div:nth-child(1) > div.panel-heading > h2 > span.date");
            }
        }

        private static string TitlePR
        {
            get
            {
                return string.Format("#{0}{1}", driver.FindElement(By.XPath("(//a[@class='point point-month'])[1]")).GetAttribute("aria-controls"), " > div:nth-child(1) > div.panel-heading > h2 > span.title");
            }
        }

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
                return driver.FindElementByCssSelector(CssSelectorAboutPR);
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
        public void TestListOfPressReleases()
        {
            Assert.IsTrue(this.BlockIsVisible());
            Assert.AreEqual(this.CountOfPressReleases(), TestNavigate.CountPressReleases);
            Assert.IsTrue(this.LoadMore());
            Assert.IsTrue(this.AboutPR());
        }

        public bool BlockIsVisible()
        {
            this.IsElementVisible(listOfReleasesXPhath);
            return driver.FindElement(listOfReleasesXPhath).Displayed;
        }

        private int CountOfPressReleases()
        {
            return driver.FindElements(pressReleaseXPath).Count;
        }

        private bool LoadMore()
        {
            driver.FindElement(buttonLoadMoreXPath).Click();
            this.IsElementVisible(buttonLoadMoreXPath);

            return driver.FindElements(PressReleasesAfterLoadingXPath).Count == 2 * TestNavigate.CountPressReleases;
        }

        private bool AboutPR()
        {
            return this.CheckTitlePR();
        }

        private bool CheckTitlePR()
        {
            var date = InnerTextOfElementFromCssSelector(By.CssSelector(TestNavigate.DatePR));
            var title = InnerTextOfElementFromCssSelector(By.CssSelector(TestNavigate.TitlePR));

            return !(string.IsNullOrWhiteSpace(date) && string.IsNullOrWhiteSpace(title));
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

        //#pressreleaselist-month-0-2017-12 > div:nth-child(1)

    }


}
