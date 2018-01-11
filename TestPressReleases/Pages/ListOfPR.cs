namespace TestPressReleases.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using OpenQA.Selenium;
    using WebDriver;

    internal class ListOfPR : BasePage
    {
        private static readonly BaseElement PressReleases = new BaseElement(By.ClassName("pressrelease-list-widget"));
        private static readonly BaseElement LoadMore = new BaseElement(By.XPath("//button[contains(@class, 'load-more-button')]"));
        private static readonly BaseElement ListOfPressReleases = new BaseElement(By.XPath("//div[@role='tablist']"));

        private static readonly BaseElement TitleOfPressReleases = new BaseElement(By.XPath("//div[@class='panel-heading']//span"));
        private static readonly BaseElement LinksOfImage = new BaseElement(By.XPath("//div[@class='panel-body']//div[@class='image']//img"));
        private static readonly string AttributeSrc = "src";

        private static readonly BaseElement Announcement = new BaseElement(By.XPath("//div[@class='tableOverflow']"));

        private static readonly BaseElement LinkToWatchPDF = new BaseElement(By.XPath("//a[contains(@class,'icon-s-flipbook-pdf_round')]"));
        private static readonly BaseElement LinkToDownloadPDF = new BaseElement(By.XPath("//a[contains(@class,'icon-s-download_round')]"));
        private static readonly BaseElement LinkToPageOfPR = new BaseElement(By.XPath("//a[contains(@class,'icon-s-chevron-link')]"));
        private static readonly string AttributeHref = "href";

        private static readonly BaseElement CalendarFrom = new BaseElement(By.Id("calendar-from"));
        private static readonly BaseElement CalendarTo = new BaseElement(By.Id("calendar-to"));
        private static readonly BaseElement FilterButtonApply = new BaseElement(By.XPath("//button[contains(@class,'filter-btn')]"));

        private static readonly BaseElement CopyRight = new BaseElement(By.XPath("//div[contains(@class,'text-right')]"));

        private string patternDate;
        private List<DateTime> dateOfPressReleases;

        internal ListOfPR() : base(PressReleases.Locator, "Press Releases")
        {
        }

        private DateTime DateFrom
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(Config.DateFrom, Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        private DateTime DateTo
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(Config.DateTo, Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        private string PatternDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.patternDate))
                {
                    this.patternDate = Config.StartUrl.Contains(".ru") ? "dd.MM.yyyy" : "MM/dd/yyyy";
                }

                return this.patternDate;
            }
        }

        internal static ListOfPR GoToPage()
        {
            var mainPage = new MainPage();
            return mainPage.GoToPageOfPressReleases();
        }

        internal bool PressReleasesIsVisible()
        {
            return ListOfPR.PressReleases.IsVisible();
        }

        internal int CountOfPressReleases()
        {
            return ListOfPR.ListOfPressReleases.FindElements(ListOfPR.ListOfPressReleases.Locator).Count;
        }

        internal ListOfPR ClickLoadMore()
        {
            for (int i = 0; i <= Config.CountOfClickMoreLoad - 1; i++)
            {
                if (ListOfPR.LoadMore.IsVisible())
                {
                    ListOfPR.LoadMore.Click();
                }
                else
                {
                    return this;
                }
            }

            return this;
        }

        internal List<string> WatchTitlesOfPressReleases()
        {
            return ListOfPR.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
        }

        internal bool IsCorrectLinkToImageOfPressReleases()
        {
            return Helper.CheckLinkOfFile(ListOfPR.LinksOfImage, ListOfPR.AttributeSrc);
        }

        internal List<string> WatchAnnouncementsOfPressReleases()
        {
            return ListOfPR.Announcement.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
        }

        internal bool IsCorrectLinkToWatchPDFOfPressReleases()
        {
            return Helper.CheckLinkOfFile(ListOfPR.LinkToWatchPDF, ListOfPR.AttributeHref);
        }

        internal bool IsCorrectLinkToDownloadPDFOfPressReleases()
        {
            return Helper.CheckLinkOfFile(ListOfPR.LinkToDownloadPDF, ListOfPR.AttributeHref);
        }

        internal bool IsCorrectTitlesOnListAndPageOfPressRelease()
        {
            return this.CheckTitleOnListAndPageOfPressRelease();
        }

        internal ListOfPR FilterByDate()
        {
            ListOfPR.CalendarFrom.SendKeys(this.DateFrom.ToString(this.PatternDate));
            ListOfPR.CalendarTo.SendKeys(this.DateTo.ToString(this.PatternDate));

            // Sometimes test failed because of button isn't clickable.
            WebDriver.GetDriver().ExecuteScript("scroll(250, 0)");
            ListOfPR.FilterButtonApply.Click();

            // Need handling of case when search found 0 items. There aren't press-releases.
            // For this to wait loading of page and to count found items.
            ListOfPR.CopyRight.IsVisible();

            if (!(ListOfPR.TitleOfPressReleases.FindElements(ListOfPR.TitleOfPressReleases.Locator).Count == 0))
            {
                // Wait loading of DOM with new elements.
                ListOfPR.TitleOfPressReleases.IsVisible();

                var titlesPROnList = ListOfPR.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
                Helper.PostHandlingForDateOfPR(titlesPROnList, false);

                this.dateOfPressReleases = new List<DateTime>();

                for (int i = 0; i < titlesPROnList.Count; i += 2)
                {
                    this.dateOfPressReleases.Add(DateTime.Parse(titlesPROnList.ElementAt(i)));
                }
            }

            return this;
        }

        internal bool MatchDateOfResultFilteringWithDateOfFilters()
        {
            return this.dateOfPressReleases.TrueForAll(date => date >= this.DateFrom && date <= this.DateTo);
        }

        private bool CheckTitleOnListAndPageOfPressRelease()
        {
            var titlesPressReleasesFromList = this.WatchTitlesOfPressReleases();

            // Click on scroll to up. Otherwise first link to page of press-release is not clickable.
            WebDriver.GetDriver().ExecuteScript("scroll(250, 0)");

            var linksToPagePR = ListOfPR.LinkToPageOfPR.FindElements(ListOfPR.LinkToPageOfPR.Locator);
            var titlesPROnPage = this.GetTitlesOfPressReleasesOnPages(linksToPagePR);

            Helper.PostHandlingForDateOfPR(titlesPressReleasesFromList, false);
            Helper.PostHandlingForDateOfPR(titlesPROnPage, true);

            return titlesPressReleasesFromList.SequenceEqual(titlesPROnPage);
        }

        private List<string> GetTitlesOfPressReleasesOnPages(ReadOnlyCollection<IWebElement> linksToPagePR)
        {
            var titlesPROnPage = new List<string>();

            for (int i = 0; i < linksToPagePR.Count; i++)
            {
                WebDriver.GetDriver().Keyboard.PressKey(Keys.Control);

                linksToPagePR[i].Click();
                var pageOfPR = WebDriver.GetDriver().SwitchTo().Window(WebDriver.GetDriver().WindowHandles.Last());

                var pageOfPressRelease = new PageOfPR();
                titlesPROnPage.AddRange(pageOfPressRelease.TitleOfPressReleaseOnPage());

                pageOfPR.Close();

                WebDriver.GetDriver().SwitchTo().Window(WebDriver.GetDriver().WindowHandles.First());

                // Press on Control otherwise link opens on current tab.
                WebDriver.GetDriver().Keyboard.PressKey(Keys.Control);
            }

            return titlesPROnPage;
        }
    }
}