namespace TestPressReleases.Pages
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WebDriver;

    internal class PageOfPR : BasePage
    {
        private static readonly BaseElement ContactList = new BaseElement(By.XPath("//div[contains(@class,'panel-contact')]//h4"));

        [FindsBy(How = How.ClassName, Using = "press-release-page-title")]
        private IWebElement titleOfPROnPage;

        [FindsBy(How = How.ClassName, Using = "pressrelise-article-date")]
        private IWebElement dateOfPROnPage;

        internal PageOfPR() : base(PageOfPR.ContactList.Locator, "Page of Press-Release")
        {
            PageFactory.InitElements(WebDriver.GetDriver(), this);
        }

        internal string[] TitleOfPressReleaseOnPage()
        {
            return new string[]
            {
                this.dateOfPROnPage.Text,
                this.titleOfPROnPage.Text
            };
        }
    }
}