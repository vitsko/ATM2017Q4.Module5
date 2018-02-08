namespace Pages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Entities;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using Utility;
    using WDriver;
    using static Entities.PressRelease;

    public class PageOfPressReleases : BasePage
    {
        public static readonly string XpathToImageOfAnnouncement = "(//div[@class='panel-body'])[{0}]//div[@class='image']//img";
        public static readonly string AttributeSrc = "src";
        public static readonly string XpathToLinkToWatchPDF = "(//a[contains(@class,'icon-s-flipbook-pdf_round')])[{0}]";
        public static readonly string XpathLinkToDownloadPDF = "(//a[contains(@class,'icon-s-download_round')])[{0}]";
        public static readonly string AttributeHref = "href";

        public static readonly BaseElement MenuPressCenter = new BaseElement(By.XPath("//li[@id='435aa1d6-2ddd-43b5-9564-3a986dd3d526']"));
        public static readonly BaseElement MenuPressReleases = new BaseElement(By.XPath(string.Format("//a[text()='{0}']", Config.MenuPressReleases)));

        private static readonly string XpathToLinkOfPageOnPressRelease = "//a[contains(@class,'icon-s-chevron-link')]";
        private static readonly string XpathToTitleOfPressRelease = "//div[@class='panel-heading']//span";

        private static readonly BaseElement PressReleases = new BaseElement(By.ClassName("pressrelease-list-widget"));
        private static readonly BaseElement TitleOfPressReleases = new BaseElement(By.XPath("//div[@class='panel-heading']//span"));
        private static readonly BaseElement Announcement = new BaseElement(By.XPath("//div[@class='tableOverflow']"));

        private string calendarFromId = "calendar-from";
        private string calendarToId = "calendar-to";

        private string patternDate;

        public PageOfPressReleases() : base(PressReleases.Locator, "Press Releases")
        {
            PageFactory.InitElements(WDriver.GetDriver(), this);
        }

        public DateTime DateFrom
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(Config.DateFrom, Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        public DateTime DateTo
        {
            get
            {
                DateTime parse;
                DateTime.TryParse(Config.DateTo, Config.Culture, DateTimeStyles.AllowWhiteSpaces, out parse);

                return parse;
            }
        }

        public string PatternDate
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

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'filter-btn')]")]
        private IWebElement FilterButtonApply { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'load-more-button')]")]
        private IWebElement LoadMore { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@role='tablist']")]
        private IWebElement TableOfPressReleases { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'icon-s-chevron-link')]")]
        private IWebElement LinkToPageOfPressRelease { get; set; }

        public List<IWebElement> PressReleasesToCount()
        {
            WDriver.WaitForIsVisible(By.XPath("//div[@role='tablist']"));

            var pressReleases = new List<IWebElement>(this.TableOfPressReleases.FindElements(By.XPath("//div[@role='tablist']")).ToList());
            return pressReleases;
        }

        public PageOfPressReleases ClickLoadMore()
        {
            for (int i = 0; i <= Config.CountOfClickMoreLoad - 1; i++)
            {
                WDriver.WaitForIsVisible(By.XPath("//button[contains(@class, 'load-more-button')]"));
                this.LoadMore.Click();
            }

            return this;
        }

        public List<PressRelease> PressReleasesWithTitle()
        {
            var pressReleases = this.PressReleasesWithId();

            int skipCount = 0;

            var by = By.XPath(PageOfPressReleases.XpathToTitleOfPressRelease);

            foreach (var pressRelease in pressReleases)
            {
                var title = WDriver.GetListValueWithSkipAndTakeElements(by, skipCount, 2);

                skipCount += 2;

                Helper.PostHandlingForDateOfPressReleases(title, false);

                pressRelease.WithTitle(title.ElementAt(0), title.ElementAt(1));
            }

            return pressReleases;
        }

        public List<PressRelease> PressReleasesWithSizeOfElement(string xpath, string attribute, SizeOfFile fileType)
        {
            var pressRelease = this.PressReleasesWithId();

            var pressReleasesTab = WDriver.GetDriver().FindElements(By.XPath("//div[@role='tablist']")).ToList();

            for (int i = 0; i < pressReleasesTab.Count; i++)
            {
                var elementsWithUrl = pressReleasesTab.ElementAt(i).FindElements(By.XPath(string.Format(xpath, i + 1)));

                if (elementsWithUrl.Count != 0)
                {
                    var url = elementsWithUrl.First().GetAttribute(attribute);

                    pressRelease.ElementAt(i).WithSizeOfFile(fileType, url);
                }
            }

            return pressRelease;
        }

        public List<PressRelease> PressReleasesWithAnnouncements()
        {
            var pressRelease = this.PressReleasesWithId();
            var announcements = PageOfPressReleases.Announcement.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);

            for (int i = 0; i < pressRelease.Count; i++)
            {
                pressRelease.ElementAt(i).WithAnnouncement(announcements.ElementAt(i));
            }

            return pressRelease;
        }

        public List<IWebElement> GetElementsOfLinkToPageOfPressRelease()
        {
            return this.LinkToPageOfPressRelease.FindElements(By.XPath("//a[contains(@class,'icon-s-chevron-link')]")).ToList();
        }

        public List<PressRelease> FilterPressReleasesByDate()
        {
            var pressReleases = new List<PressRelease>();

            WDriver.SetValueByScript("Id", this.calendarFromId, this.DateFrom.ToString(this.PatternDate));
            WDriver.SetValueByScript("Id", this.calendarToId, this.DateTo.ToString(this.PatternDate));

            // Sometimes test failed because of button isn't clickable.
            WDriver.GetDriver().ExecuteScript("scroll(250, 0)");
            this.FilterButtonApply.Click();

            // Need handling of case when search found 0 items. There aren't press-releases.
            // For this to wait loading of page and to count found items.
            WDriver.WaitForIsVisible(By.XPath("//div[contains(@class,'text-right')]"));

            if (PageOfPressReleases.TitleOfPressReleases.FindElements(PageOfPressReleases.TitleOfPressReleases.Locator).Count != 0)
            {
                // Wait loading of DOM with new elements.
                PageOfPressReleases.TitleOfPressReleases.IsVisible();
                return this.PressReleasesWithDate();
            }

            return pressReleases;
        }

        private List<PressRelease> PressReleasesWithId()
        {
            var pressReleases = new List<PressRelease>();

            if (WDriver.GetDriver().FindElements(By.XPath(PageOfPressReleases.XpathToLinkOfPageOnPressRelease)).Count != 0)
            {
                var allTabWithPressReleases = WDriver.GetDriver().FindElements(By.XPath(PageOfPressReleases.XpathToLinkOfPageOnPressRelease));

                foreach (var elementWithId in allTabWithPressReleases)
                {
                    pressReleases.Add(new PressRelease(int.Parse(elementWithId.GetAttribute(PageOfPressReleases.AttributeHref)
                                                                                           .Replace(
                                                                                                    Config.PatternToUrlOnPageOfPressRelease,
                                                                                                    string.Empty))));
                }
            }

            return pressReleases;
        }

        private List<PressRelease> PressReleasesWithDate()
        {
            var pressReleases = this.PressReleasesWithId();

            var titlesAndDatePressReleasesOnList = PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
            Helper.PostHandlingForDateOfPressReleases(titlesAndDatePressReleasesOnList, false);

            List<string> onlyDate = Helper.GetListWithOnlySomeDeltaOfIndex(titlesAndDatePressReleasesOnList, 2);

            for (int i = 0; i < pressReleases.Count; i++)
            {
                pressReleases.ElementAt(i).WithDate(DateTime.Parse(onlyDate.ElementAt(i)));
            }

            return pressReleases;
        }
    }
}