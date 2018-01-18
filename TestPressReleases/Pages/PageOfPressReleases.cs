namespace TestPressReleases.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WebDriver;

    internal class PageOfPressReleases : BasePage
    {
        internal static readonly string XpathToImageOfAnnouncement = "(//div[@class='panel-body'])[{0}]//div[@class='image']//img";
        internal static readonly string AttributeSrc = "src";
        internal static readonly string XpathToLinkToWatchPDF = "(//a[contains(@class,'icon-s-flipbook-pdf_round')])[{0}]";
        internal static readonly string XpathLinkToDownloadPDF = "(//a[contains(@class,'icon-s-download_round')])[{0}]";
        internal static readonly string AttributeHref = "href";

        internal static readonly BaseElement MenuPressCenter = new BaseElement(By.XPath("//li[@id='435aa1d6-2ddd-43b5-9564-3a986dd3d526']"));
        internal static readonly BaseElement MenuPressReleases = new BaseElement(By.XPath(string.Format("//a[text()='{0}']", Config.MenuPressReleases)));

        private static readonly BaseElement PressReleases = new BaseElement(By.ClassName("pressrelease-list-widget"));
        private static readonly BaseElement TitleOfPressReleases = new BaseElement(By.XPath("//div[@class='panel-heading']//span"));
        private static readonly BaseElement Announcement = new BaseElement(By.XPath("//div[@class='tableOverflow']"));

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'load-more-button')]")]
        private IWebElement loadMore;

        [FindsBy(How = How.XPath, Using = "//div[@role='tablist']")]
        private IWebElement tableOfPressReleases;

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'icon-s-chevron-link')]")]
        private IWebElement linkToPageOfPressRelease;

        private string calendarFromId = "calendar-from";
        private string calendarToId = "calendar-to";

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'filter-btn')]")]
        private IWebElement filterButtonApply;

        private string patternDate;

        internal PageOfPressReleases() : base(PressReleases.Locator, "Press Releases")
        {
            PageFactory.InitElements(WebDriver.GetDriver(), this);
        }

        internal DateTime DateFrom
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(Config.DateFrom, Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        internal DateTime DateTo
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(Config.DateTo, Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        internal string PatternDate
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
            var titles = PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);

            Helper.PostHandlingForDateOfPressReleases(titles, false);
            Helper.JoinStringsInListByPair(titles);

            return titles;
        }

        internal Dictionary<string, long> GetSizeOfElementForPressReleases(string xpath, string attribute)
        {
            var titles = this.WatchTitlesOfPressReleases();
            var pressReleasesTab = WebDriver.GetDriver().FindElements(By.XPath("//div[@role='tablist']")).ToList();

            Dictionary<string, long> titleOfPressReleaseAndSizeOfElement = new Dictionary<string, long>();

            for (int i = 0; i < pressReleasesTab.Count; i++)
            {
                var elementsWithUrl = pressReleasesTab.ElementAt(i).FindElements(By.XPath(string.Format(xpath, i + 1)));

                if (elementsWithUrl.Count != 0)
                {
                    var url = elementsWithUrl.First().GetAttribute(attribute);
                    var contentLength = Helper.GetContentLengthByLink(url);

                    titleOfPressReleaseAndSizeOfElement.Add(titles.ElementAt(i), contentLength);
                }
            }

            return titleOfPressReleaseAndSizeOfElement;
        }

        internal Dictionary<string, string> GetAnnouncementsOfPressReleases()
        {
            var titles = this.WatchTitlesOfPressReleases();
            var announcements = PageOfPressReleases.Announcement.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);

            Dictionary<string, string> titlesAndAnnouncements = new Dictionary<string, string>();

            for (int i = 0; i < titles.Count; i++)
            {
                titlesAndAnnouncements.Add(titles.ElementAt(i), announcements.ElementAt(i));
            }

            return titlesAndAnnouncements;
        }

        internal List<IWebElement> GetElementsOfLinkToPageOfPressRelease()
        {
            return this.linkToPageOfPressRelease.FindElements(By.XPath("//a[contains(@class,'icon-s-chevron-link')]")).ToList();
        }

        internal Dictionary<string, DateTime> FilterByDate()
        {
            WebDriver.SetValueByScript("Id", this.calendarFromId, this.DateFrom.ToString(this.PatternDate));
            WebDriver.SetValueByScript("Id", this.calendarToId, this.DateTo.ToString(this.PatternDate));

            // Sometimes test failed because of button isn't clickable.
            WebDriver.GetDriver().ExecuteScript("scroll(250, 0)");
            this.filterButtonApply.Click();

            // Need handling of case when search found 0 items. There aren't press-releases.
            // For this to wait loading of page and to count found items.
            WebDriver.WaitForIsVisible(By.XPath("//div[contains(@class,'text-right')]"));

            var titleAndDatime = new Dictionary<string, DateTime>();

            if (PageOfPressReleases.TitleOfPressReleases.FindElements(PageOfPressReleases.TitleOfPressReleases.Locator).Count != 0)
            {
                // Wait loading of DOM with new elements.
                PageOfPressReleases.TitleOfPressReleases.IsVisible();

                var textOfTitle = this.WatchTitlesOfPressReleases();

                var titlesAndDatePressReleasesOnList = PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
                Helper.PostHandlingForDateOfPressReleases(titlesAndDatePressReleasesOnList, false);

                List<string> onlyDate = Helper.GetListWithOnlySomeDeltaOfIndex(titlesAndDatePressReleasesOnList, 2);

                for (int i = 0; i < textOfTitle.Count; i++)
                {
                    titleAndDatime.Add(textOfTitle.ElementAt(i), DateTime.Parse(onlyDate.ElementAt(i)));
                }
            }

            return titleAndDatime;
        }
    }
}