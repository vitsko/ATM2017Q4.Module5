namespace TestPressReleases.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WebDriver;

    internal class PageOfPressRelease : BasePage
    {
        private static readonly BaseElement ContactList = new BaseElement(By.XPath("//div[contains(@class,'panel-contact')]//h4"));

        internal PageOfPressRelease() : base(PageOfPressRelease.ContactList.Locator, "Page of Press-Release")
        {
            PageFactory.InitElements(WebDriver.GetDriver(), this);
        }

        [FindsBy(How = How.ClassName, Using = "press-release-page-title")]
        private IWebElement TitleOfPressReleaseOnPage { get; set; }

        [FindsBy(How = How.ClassName, Using = "pressrelise-article-date")]
        private IWebElement DateOfPressReleaseOnPage { get; set; }

        internal static List<string> GetTitlesOfPressRelease(List<IWebElement> elementsWithLink)
        {
            var titlesOfPressReleasesOnPage = new List<string>();

            foreach (var link in elementsWithLink)
            {
                var windowsHandels = new List<string>();
                var tab = WebDriver.OpenLinkInNewTab(link, out windowsHandels);
                var pageOfPressRelease = new PageOfPressRelease();
                titlesOfPressReleasesOnPage.AddRange(pageOfPressRelease.GetTitleOfPressReleaseOnPage());

                tab.Close();

                WebDriver.GetDriver().SwitchTo().Window(windowsHandels.First());

                // Press on Control otherwise link opens on current tab.
                WebDriver.GetDriver().Keyboard.PressKey(Keys.Control);
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