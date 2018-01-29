namespace Pages
{
    using OpenQA.Selenium;
    using WDriver;

    public class BasePage
    {
        protected BasePage(By titleLocator, string title)
        {
            this.TitleLocator = titleLocator;
            this.Title = title;
            this.AssertIsOpen();
        }

        protected By TitleLocator { get; set; }

        protected string Title { get; set; }

        private void AssertIsOpen()
        {
            var label = new BaseElement(this.TitleLocator, this.Title);
            WDriver.WaitForIsVisible(this.TitleLocator);
        }
    }
}