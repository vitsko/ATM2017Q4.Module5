namespace TestPressReleases.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WebDriver;

    internal class PageOfPressReleases : BasePage
    {
        internal static readonly BaseElement MenuPressCenter = new BaseElement(By.XPath("//li[@id='435aa1d6-2ddd-43b5-9564-3a986dd3d526']"));
        internal static readonly BaseElement MenuPressReleases = new BaseElement(By.XPath(string.Format("//a[text()='{0}']", Config.MenuPressReleases)));
        internal static readonly BaseElement PressReleases = new BaseElement(By.ClassName("pressrelease-list-widget"));

        private static readonly BaseElement TitleOfPressReleases = new BaseElement(By.XPath("//div[@class='panel-heading']//span"));
        private static readonly BaseElement LinksOfImage = new BaseElement(By.XPath("//div[@class='panel-body']//div[@class='image']//img"));
        private static readonly string AttributeSrc = "src";
        private static readonly BaseElement Announcement = new BaseElement(By.XPath("//div[@class='tableOverflow']"));
        private static readonly BaseElement LinkToWatchPDF = new BaseElement(By.XPath("//a[contains(@class,'icon-s-flipbook-pdf_round')]"));
        private static readonly BaseElement LinkToDownloadPDF = new BaseElement(By.XPath("//a[contains(@class,'icon-s-download_round')]"));
        private static readonly string AttributeHref = "href";

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'load-more-button')]")]
        private IWebElement loadMore;

        [FindsBy(How = How.XPath, Using = "//div[@role='tablist']")]
        private IWebElement tableOfPressReleases;

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'icon-s-chevron-link')]")]
        private IWebElement linkToPageOfPressRelease;

        [FindsBy(How = How.Id, Using = "calendar-from")]
        private IWebElement calendarFrom;

        [FindsBy(How = How.Id, Using = "calendar-to")]
        private IWebElement calendarTo;

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'filter-btn')]")]
        private IWebElement filterButtonApply;

        private string patternDate;
        private List<DateTime> dateOfPressReleases;

        internal PageOfPressReleases() : base(PressReleases.Locator, "Press Releases")
        {
            PageFactory.InitElements(WebDriver.GetDriver(), this);
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

        internal bool PressReleasesIsVisible()
        {
            return PageOfPressReleases.PressReleases.IsVisible();
        }

        internal int CountOfPressReleases()
        {
            WebDriver.WaitForIsVisible(By.XPath("//div[@role='tablist']"));
            return this.tableOfPressReleases.FindElements(By.XPath("//div[@role='tablist']")).Count;
        }

        internal PageOfPressReleases ClickLoadMore()
        {
            for (int i = 0; i <= Config.CountOfClickMoreLoad - 1; i++)
            {
                WebDriver.WaitForIsVisible(By.XPath("//button[contains(@class, 'load-more-button')]"));
                this.loadMore.Click();
            }

            return this;
        }

        internal List<string> WatchTitlesOfPressReleases()
        {
            return PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
        }

        internal bool IsCorrectLinkToImageOfPressReleases()
        {
            return Helper.CheckLinkOfFile(PageOfPressReleases.LinksOfImage, PageOfPressReleases.AttributeSrc);
        }

        internal List<string> WatchAnnouncementsOfPressReleases()
        {
            return PageOfPressReleases.Announcement.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
        }

        internal bool IsCorrectLinkToWatchPDFOfPressReleases()
        {
            return Helper.CheckLinkOfFile(PageOfPressReleases.LinkToWatchPDF, PageOfPressReleases.AttributeHref);
        }

        internal bool IsCorrectLinkToDownloadPDFOfPressReleases()
        {
            return Helper.CheckLinkOfFile(PageOfPressReleases.LinkToDownloadPDF, PageOfPressReleases.AttributeHref);
        }

        internal bool IsCorrectTitlesOnListAndPageOfPressRelease()
        {
            return this.CheckTitleOnListAndPageOfPressRelease();
        }

        internal PageOfPressReleases FilterByDate()
        {
            this.calendarFrom.SendKeys(this.DateFrom.ToString(this.PatternDate));
            this.calendarTo.SendKeys(this.DateTo.ToString(this.PatternDate));

            // Sometimes test failed because of button isn't clickable.
            WebDriver.GetDriver().ExecuteScript("scroll(250, 0)");
            this.filterButtonApply.Click();

            // Need handling of case when search found 0 items. There aren't press-releases.
            // For this to wait loading of page and to count found items.
            WebDriver.WaitForIsVisible(By.XPath("//div[contains(@class,'text-right')]"));

            if (PageOfPressReleases.TitleOfPressReleases.FindElements(PageOfPressReleases.TitleOfPressReleases.Locator).Count != 0)
            {
                // Wait loading of DOM with new elements.
                PageOfPressReleases.TitleOfPressReleases.IsVisible();

                var titlesPressReleasesOnList = PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
                Helper.PostHandlingForDateOfPressReleases(titlesPressReleasesOnList, false);

                this.dateOfPressReleases = new List<DateTime>();

                for (int i = 0; i < titlesPressReleasesOnList.Count; i += 2)
                {
                    this.dateOfPressReleases.Add(DateTime.Parse(titlesPressReleasesOnList.ElementAt(i)));
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

            var linksToPageOfPressReleases = this.linkToPageOfPressRelease.FindElements(By.XPath("//a[contains(@class,'icon-s-chevron-link')]"));
            var titlesOfPressReleasesOnPage = this.GetTitlesOfPressReleasesOnPages(linksToPageOfPressReleases);

            Helper.PostHandlingForDateOfPressReleases(titlesPressReleasesFromList, false);
            Helper.PostHandlingForDateOfPressReleases(titlesOfPressReleasesOnPage, true);

            return titlesPressReleasesFromList.SequenceEqual(titlesOfPressReleasesOnPage);
        }

        private List<string> GetTitlesOfPressReleasesOnPages(ReadOnlyCollection<IWebElement> linksToPageOfPressRelease)
        {
            var titlesOfPressReleasesOnPage = new List<string>();

            for (int i = 0; i < linksToPageOfPressRelease.Count; i++)
            {
                WebDriver.GetDriver().Keyboard.PressKey(Keys.Control);

                linksToPageOfPressRelease[i].Click();

                var windowsHandles = WebDriver.GetDriver().WindowHandles;
                var webDriverForPageOfPressRelease = WebDriver.GetDriver().SwitchTo().Window(windowsHandles.Last());

                var pageOfPressRelease = new PageOfPressRelease();
                titlesOfPressReleasesOnPage.AddRange(pageOfPressRelease.TitleOfPressReleaseOnPage());

                webDriverForPageOfPressRelease.Close();

                WebDriver.GetDriver().SwitchTo().Window(windowsHandles.First());

                // Press on Control otherwise link opens on current tab.
                WebDriver.GetDriver().Keyboard.PressKey(Keys.Control);
            }

            return titlesOfPressReleasesOnPage;
        }
    }
}