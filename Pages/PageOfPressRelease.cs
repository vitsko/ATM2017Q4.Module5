namespace Pages
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using WDriver;

    public class PageOfPressRelease : BasePage
    {
        private static readonly BaseElement ContactList = new BaseElement(By.XPath("//div[@class='copy text-right']"));

        public PageOfPressRelease() : base(PageOfPressRelease.ContactList.Locator, "Page of Press-Release")
        {
            PageFactory.InitElements(WDriver.GetDriver(), this);
        }

        [FindsBy(How = How.ClassName, Using = "press-release-page-title")]
        private IWebElement TitleOfPressReleaseOnPage { get; set; }

        [FindsBy(How = How.ClassName, Using = "pressrelise-article-date")]
        private IWebElement DateOfPressReleaseOnPage { get; set; }

        public string[] GetTitleOfPressReleaseOnPage()
        {
            return new string[]
            {
                this.DateOfPressReleaseOnPage.Text,
                this.TitleOfPressReleaseOnPage.Text
            };
        }
    }
}