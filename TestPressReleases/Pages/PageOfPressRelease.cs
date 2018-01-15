namespace TestPressReleases.Pages
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WebDriver;

    internal class PageOfPressRelease : BasePage
    {
        private static readonly BaseElement ContactList = new BaseElement(By.XPath("//div[contains(@class,'panel-contact')]//h4"));

        [FindsBy(How = How.ClassName, Using = "press-release-page-title")]
        private IWebElement titleOfPressReleaseOnPage;

        [FindsBy(How = How.ClassName, Using = "pressrelise-article-date")]
        private IWebElement dateOfPressReleaseOnPage;

        internal PageOfPressRelease() : base(PageOfPressRelease.ContactList.Locator, "Page of Press-Release")
        {
            PageFactory.InitElements(WebDriver.GetDriver(), this);
        }

        internal string[] TitleOfPressReleaseOnPage()
        {
            return new string[]
            {
                this.dateOfPressReleaseOnPage.Text,
                this.titleOfPressReleaseOnPage.Text
            };
        }
    }
}