namespace Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using Utility;
    using WDriver;

    public class PageOfPressRelease : BasePage
    {
        private static readonly BaseElement ContactList = new BaseElement(By.XPath("//div[contains(@class,'panel-contact')]//h4"));

        internal PageOfPressRelease() : base(PageOfPressRelease.ContactList.Locator, "Page of Press-Release")
        {
            PageFactory.InitElements(WDriver.GetDriver(), this);
        }

        [FindsBy(How = How.ClassName, Using = "press-release-page-title")]
        private IWebElement TitleOfPressReleaseOnPage { get; set; }

        [FindsBy(How = How.ClassName, Using = "pressrelise-article-date")]
        private IWebElement DateOfPressReleaseOnPage { get; set; }

        public static List<string> GetTitlesOfPressRelease(List<IWebElement> elementsWithLink)
        {
            var titlesOfPressReleasesOnPage = new List<string>();

            foreach (var link in elementsWithLink)
            {
                var windowsHandels = new List<string>();
                var tab = WDriver.OpenLinkInNewTab(link, out windowsHandels);
                var pageOfPressRelease = new PageOfPressRelease();
                titlesOfPressReleasesOnPage.AddRange(pageOfPressRelease.GetTitleOfPressReleaseOnPage());

                tab.Close();

                WDriver.GetDriver().SwitchTo().Window(windowsHandels.First());

                // Press on Control otherwise link opens on current tab.
                WDriver.GetDriver().Keyboard.PressKey(Keys.Control);
            }

            Helper.PostHandlingForDateOfPressReleases(titlesOfPressReleasesOnPage, true);
            Helper.JoinStringsInListByPair(titlesOfPressReleasesOnPage);

            return titlesOfPressReleasesOnPage;
        }

        private string[] GetTitleOfPressReleaseOnPage()
        {
            return new string[]
            {
                this.DateOfPressReleaseOnPage.Text,
                this.TitleOfPressReleaseOnPage.Text
            };
        }
    }
}