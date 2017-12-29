namespace TestPressReleases
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;

    [TestFixture]
    public class TestNavigate
    {
        private static ChromeDriver driver = new ChromeDriver();

        private static string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private static int timeoutInSec = int.Parse(ConfigurationManager.AppSettings["TimeoutInSec"]);
        private static int countOfPressReleases = int.Parse(ConfigurationManager.AppSettings["CountOfPressReleases"]);
        private static int countOfClickMoreLoad = int.Parse(ConfigurationManager.AppSettings["CountOfClickMoreLoad"]);

        private By listOfReleasesXPhath = By.XPath("//div[@class='pressrelease-list-widget']");
        private By pressReleaseXPath = By.XPath("//div[@role='tablist']");

        private By pressReleasesAfterLoadingXPath = By.XPath("//div[contains(@class,'panel-collapsible')]");

        private By buttonLoadMoreXPath = By.XPath("//button[@data-bind='visible: CanLoadMore']");

        private string cssHeaderPR = "div.panel-default.panel-time-line.panel-collapsible>div.panel-heading>h2>span";
        private string cssImagePR = "div.panel-default.panel-time-line.panel-collapsible div.image img";
        private string srcImage = "src";

        private string cssAnnouncement = "div.panel-default.panel-time-line.panel-collapsible div.tableOverflow";

        private string cssLinkToWatchPDF = "div.panel-default.panel-time-line.panel-collapsible a.icon-s-flipbook-pdf_round";
        private string cssLinkToDownloadPDF = "div.panel-default.panel-time-line.panel-collapsible a.icon-s-download_round";
        private string href = "href";

        private int CountOfElements { get; set; }

        private int MaxCountOfPROnPage
        {
            get
            {
                return (TestNavigate.countOfClickMoreLoad + 1) * TestNavigate.countOfPressReleases;
            }
        }

        [SetUp]
        public void SetupTest()
        {
            driver.Navigate().GoToUrl(baseUrl);
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
            Assert.AreEqual(this.DefaultCountOfPressReleases(), TestNavigate.countOfPressReleases);
            Assert.IsTrue(this.LoadMore() <= this.MaxCountOfPROnPage);
            Assert.IsTrue(this.AboutPR());
        }

        public bool BlockIsVisible()
        {
            this.IsElementVisible(this.listOfReleasesXPhath);
            return driver.FindElement(this.listOfReleasesXPhath).Displayed;
        }

        private int DefaultCountOfPressReleases()
        {
            return driver.FindElements(this.pressReleaseXPath).Count;
        }

        private int LoadMore()
        {
            try
            {
                for (int i = 0; i <= TestNavigate.countOfClickMoreLoad - 1; i++)
                {
                    this.IsElementVisible(this.buttonLoadMoreXPath);
                    driver.FindElement(this.buttonLoadMoreXPath).Click();
                    this.IsElementVisible(this.buttonLoadMoreXPath);
                }
            }
            catch { }

            return driver.FindElements(this.pressReleasesAfterLoadingXPath).Count;
        }

        private bool AboutPR()
        {
            return this.IsExistTextByCSS(this.cssHeaderPR) && (this.CountOfElements / 2 <= this.MaxCountOfPROnPage)
                   && this.CheckLinkOfFile(this.cssImagePR, this.srcImage) && (this.CountOfElements <= this.MaxCountOfPROnPage)
                   && this.IsExistTextByCSS(this.cssAnnouncement) && (this.CountOfElements <= this.MaxCountOfPROnPage)
                   && this.CheckLinkOfFile(this.cssLinkToWatchPDF, this.href) && (this.CountOfElements <= this.MaxCountOfPROnPage)
                   && this.CheckLinkOfFile(this.cssLinkToDownloadPDF, this.href) && (this.CountOfElements <= this.MaxCountOfPROnPage);
        }

        private bool IsExistTextByCSS(string cssSelector)
        {
            return this.InnerTextOfElementsFromCssSelector(cssSelector).TrueForAll(item => !string.IsNullOrWhiteSpace(item));
        }

        private List<string> InnerTextOfElementsFromCssSelector(string cssSelector)
        {
            var text = new List<string>();

            foreach (var item in this.GetAllElementsByCssSelector(cssSelector))
            {
                text.Add(item.Text);
            }

            this.CountOfElements = text.Count;

            return text;
        }

        private bool CheckLinkOfFile(string cssSelector, string attribute)
        {
            var urls = this.GetValueOfAttribute(cssSelector, attribute);

            WebRequest request;
            HttpWebResponse response;

            var sizes = new List<long>();

            for (int i = 0; i < urls.Count; i++)
            {
                request = WebRequest.Create(urls.ElementAt(i));
                response = (HttpWebResponse)request.GetResponse();

                sizes.Add(response.ContentLength);
            }

            this.CountOfElements = sizes.Count;

            return sizes.TrueForAll(size => size > 0);
        }

        private List<string> GetValueOfAttribute(string cssSelector, string attribute)
        {
            var value = new List<string>();

            foreach (var item in this.GetAllElementsByCssSelector(cssSelector))
            {
                value.Add(item.GetAttribute(attribute));
            }

            return value;
        }

        private ReadOnlyCollection<IWebElement> GetAllElementsByCssSelector(string cssSelector)
        {
            return driver.FindElementsByCssSelector(cssSelector);
        }

        private void IsElementVisible(By element)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSec)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
        }
    }
}