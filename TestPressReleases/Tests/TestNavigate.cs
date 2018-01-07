namespace TestPressReleases
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using Pages;
    using Tests;

    [TestFixture]
    internal class TestNavigate : BaseTest
    {
        private static ChromeDriver driver;
        private static CultureInfo culture;
        private static string baseUrl = ConfigurationManager.AppSettings["baseUrl"];

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
        private string cssLinkToPagePR = "div.panel-default.panel-time-line.panel-collapsible a.icon-s-chevron-link";
        private string href = "href";

        private string cssTitleOfPROnpage = "h3.press-release-page-title";
        private string cssDateOfPROnpage = "span.pressrelise-article-date";

        private string cssDateFrom = "input[id='calendar-from']";
        private string cssDateTo = "input[id='calendar-to']";
        private string cssButtonFilter = "button.filter-btn";
        private string cssCopyRight = "div.copy";

        private string patternDate;

        internal static ChromeDriver Driver
        {
            get
            {
                if (TestNavigate.driver == null)
                {
                    TestNavigate.driver = new ChromeDriver();
                }

                return TestNavigate.driver;
            }
        }

        internal static int CountOfElements { get; set; }

        internal static CultureInfo Culture
        {
            get
            {
                if (TestNavigate.culture == null)
                {
                    TestNavigate.culture = TestNavigate.baseUrl.Contains(".ru") ? new CultureInfo("ru-RU") : new CultureInfo("en-US");
                }

                return TestNavigate.culture;
            }
        }

        private int MaxCountOfPROnPage
        {
            get
            {
                return (TestNavigate.countOfClickMoreLoad + 1) * TestNavigate.countOfPressReleases;
            }
        }

        private DateTime DateFrom
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(ConfigurationManager.AppSettings["DateFrom"], TestNavigate.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        private DateTime DateTo
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(ConfigurationManager.AppSettings["DateTo"], TestNavigate.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        private string PatternDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.patternDate))
                {
                    this.patternDate = TestNavigate.baseUrl.Contains(".ru") ? "dd.MM.yyyy" : "MM/dd/yyyy";
                }

                return this.patternDate;
            }
        }

        //[Test]
        public void TestListOfPressReleases()
        {
            Assert.IsTrue(this.AboutPR());
            Assert.IsTrue(this.CheckRangeDateOfPRByFilter());
        }

        [Test]
        public void Test()
        {
            var listPR = new ListOfPR();
            listPR.ClickLoadMore();
            listPR.WatchInfoOfPressReleases();
        }



        private bool AboutPR()
        {
            return this.CheckTitleOnListAndPagePR();
        }

        private bool CheckTitleOnListAndPagePR()
        {
            var titlesPROnList = Helper.InnerTextOfElementsFromCssSelector(this.cssHeaderPR);

            // Click on scroll to up. Otherwise first link to page of press-release is not clickable.
            TestNavigate.Driver.ExecuteScript("scroll(250, 0)");

            var linksToPagePR = TestNavigate.Driver.FindElementsByCssSelector(this.cssLinkToPagePR);
            var titlesPROnPage = this.GetTitlesOfPROnPages(linksToPagePR);

            Helper.PostHandlingForDateOfPR(titlesPROnList, false);
            Helper.PostHandlingForDateOfPR(titlesPROnPage, true);

            return titlesPROnList.SequenceEqual(titlesPROnPage);
        }

        private List<string> GetTitlesOfPROnPages(ReadOnlyCollection<IWebElement> linksToPagePR)
        {
            var titlesPROnPage = new List<string>();

            for (int i = 0; i < linksToPagePR.Count; i++)
            {
                TestNavigate.Driver.Keyboard.PressKey(Keys.Control);

                linksToPagePR[i].Click();
                var pageOfPR = TestNavigate.Driver.SwitchTo().Window(TestNavigate.Driver.WindowHandles.Last());

                this.TitleOfPROnPage(titlesPROnPage);

                pageOfPR.Close();

                TestNavigate.Driver.SwitchTo().Window(TestNavigate.Driver.WindowHandles.First());

                // Press on Control otherwise link opens on current tab.
                TestNavigate.Driver.Keyboard.PressKey(Keys.Control);
            }

            return titlesPROnPage;
        }

        private void TitleOfPROnPage(List<string> titlesPROnPage)
        {
            titlesPROnPage.Add(Helper.InnerTextOfElementsFromCssSelector(this.cssTitleOfPROnpage).First());
            var date = Helper.InnerTextOfElementsFromCssSelector(this.cssDateOfPROnpage).First();

            titlesPROnPage.Insert(titlesPROnPage.Count - 1, date);
        }

        private bool CheckRangeDateOfPRByFilter()
        {
            var fromDate = Helper.GetAllElementsByCssSelector(this.cssDateFrom).First();
            var toDate = Helper.GetAllElementsByCssSelector(this.cssDateTo).First();

            fromDate.SendKeys(this.DateFrom.ToString(this.PatternDate));
            toDate.SendKeys(this.DateTo.ToString(this.PatternDate));

            var button = Helper.GetAllElementsByCssSelector(this.cssButtonFilter).First();

            // Sometimes test failed because of button isn't clickable.
            TestNavigate.Driver.ExecuteScript("scroll(250, 0)");
            button.Click();

            // Need handling of case when search found 0 items. There aren't press-releases.
            // For this to wait loading of page and to count found items.
            Helper.IsElementVisible(By.CssSelector(this.cssCopyRight));

            if (!(Helper.GetAllElementsByCssSelector(this.cssHeaderPR).Count == 0))
            {
                // Wait loading of DOM with new elements.
                Helper.IsElementVisible(By.CssSelector(this.cssHeaderPR));

                var titlesPROnList = Helper.InnerTextOfElementsFromCssSelector(this.cssHeaderPR);
                Helper.PostHandlingForDateOfPR(titlesPROnList, false);

                var dateOfPR = new List<DateTime>();

                for (int i = 0; i < titlesPROnList.Count; i += 2)
                {
                    dateOfPR.Add(DateTime.Parse(titlesPROnList.ElementAt(i)));
                }

                return dateOfPR.TrueForAll(date => date >= this.DateFrom && date <= this.DateTo);
            }

            return true;
        }
    }
}