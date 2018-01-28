﻿namespace TestPressReleases.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Entities;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WebDriver;
    using static Entities.PressRelease;

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

        private string calendarFromId = "calendar-from";
        private string calendarToId = "calendar-to";

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

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'filter-btn')]")]
        private IWebElement FilterButtonApply { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class, 'load-more-button')]")]
        private IWebElement LoadMore { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@role='tablist']")]
        private IWebElement TableOfPressReleases { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'icon-s-chevron-link')]")]
        private IWebElement LinkToPageOfPressRelease { get; set; }

        internal List<IWebElement> PressReleasesToCount()
        {
            WebDriver.WaitForIsVisible(By.XPath("//div[@role='tablist']"));

            var pressReleases = new List<IWebElement>(this.TableOfPressReleases.FindElements(By.XPath("//div[@role='tablist']")).ToList());
            return pressReleases;
        }

        internal PageOfPressReleases ClickLoadMore()
        {
            for (int i = 0; i <= Config.CountOfClickMoreLoad - 1; i++)
            {
                WebDriver.WaitForIsVisible(By.XPath("//button[contains(@class, 'load-more-button')]"));
                this.LoadMore.Click();
            }

            return this;
        }

        internal List<PressRelease> PressReleasesWithTitle()
        {
            var titlesOfPressReleases = PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
            Helper.PostHandlingForDateOfPressReleases(titlesOfPressReleases, false);

            var pressReleases = this.PressReleasesWithId();

            foreach (var pressReleas in pressReleases)
            {
                pressReleas.Title = new string[]
                {
                    titlesOfPressReleases.ElementAt(0),
                    titlesOfPressReleases.ElementAt(1)
                };

                titlesOfPressReleases.RemoveRange(0, 2);
                titlesOfPressReleases.Add(string.Empty);
            }

            return pressReleases;
        }

        internal List<PressRelease> PressReleasesWithSizeOfElement(string xpath, string attribute, SizeOfFile fileType)
        {
            var pressRelease = this.PressReleasesWithId();

            var pressReleasesTab = WebDriver.GetDriver().FindElements(By.XPath("//div[@role='tablist']")).ToList();

            for (int i = 0; i < pressReleasesTab.Count; i++)
            {
                var elementsWithUrl = pressReleasesTab.ElementAt(i).FindElements(By.XPath(string.Format(xpath, i + 1)));

                if (elementsWithUrl.Count != 0)
                {
                    var url = elementsWithUrl.First().GetAttribute(attribute);

                    switch (fileType)
                    {
                        case SizeOfFile.Image:
                            pressRelease.ElementAt(i).SizeOfImageByAnnouncement = Helper.GetContentLengthByLink(url);
                            break;

                        case SizeOfFile.WatchPDF:
                            pressRelease.ElementAt(i).SizeOfFileToWatchPDF = Helper.GetContentLengthByLink(url);
                            break;

                        case SizeOfFile.DownloadPDF:
                            pressRelease.ElementAt(i).SizeOfFileToDownloadPDF = Helper.GetContentLengthByLink(url);
                            break;
                    }
                }
            }

            return pressRelease;
        }

        internal List<PressRelease> PressReleasesWithAnnouncements()
        {
            var pressRelease = this.PressReleasesWithId();
            var announcements = PageOfPressReleases.Announcement.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);

            for (int i = 0; i < pressRelease.Count; i++)
            {
                pressRelease.ElementAt(i).Announcement = announcements.ElementAt(i);
            }

            return pressRelease;
        }

        internal List<IWebElement> GetElementsOfLinkToPageOfPressRelease()
        {
            return this.LinkToPageOfPressRelease.FindElements(By.XPath("//a[contains(@class,'icon-s-chevron-link')]")).ToList();
        }

        internal List<PressRelease> FilterPressReleasesByDate()
        {
            var pressReleases = new List<PressRelease>();

            WebDriver.SetValueByScript("Id", this.calendarFromId, this.DateFrom.ToString(this.PatternDate));
            WebDriver.SetValueByScript("Id", this.calendarToId, this.DateTo.ToString(this.PatternDate));

            // Sometimes test failed because of button isn't clickable.
            WebDriver.GetDriver().ExecuteScript("scroll(250, 0)");
            this.FilterButtonApply.Click();

            // Need handling of case when search found 0 items. There aren't press-releases.
            // For this to wait loading of page and to count found items.
            WebDriver.WaitForIsVisible(By.XPath("//div[contains(@class,'text-right')]"));

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
            var listOfIdForPressReleases = this.GetListOfIdForPressReleases();

            var pressReleases = new List<PressRelease>();

            for (int i = 0; i < listOfIdForPressReleases.Count; i++)
            {
                var pressReleas = new PressRelease();
                pressReleas.Id = listOfIdForPressReleases.ElementAt(i);

                pressReleases.Add(pressReleas);
            }

            return pressReleases;
        }

        private List<int> GetListOfIdForPressReleases()
        {
            var elementsWithLink = this.GetElementsOfLinkToPageOfPressRelease();
            var listOfIdForPressReleases = new List<int>();

            foreach (var element in elementsWithLink)
            {
                listOfIdForPressReleases.Add(int.Parse(element.GetAttribute(PageOfPressReleases.AttributeHref).Replace(Config.PatternToUrlOnPageOfPressRelease, string.Empty)));
            }

            return listOfIdForPressReleases;
        }

        private List<PressRelease> PressReleasesWithDate()
        {
            var pressReleases = this.PressReleasesWithId();

            var titlesAndDatePressReleasesOnList = PageOfPressReleases.TitleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);
            Helper.PostHandlingForDateOfPressReleases(titlesAndDatePressReleasesOnList, false);

            List<string> onlyDate = Helper.GetListWithOnlySomeDeltaOfIndex(titlesAndDatePressReleasesOnList, 2);

            for (int i = 0; i < pressReleases.Count; i++)
            {
                pressReleases.ElementAt(i).Date = DateTime.Parse(onlyDate.ElementAt(i));
            }

            return pressReleases;
        }
    }
}