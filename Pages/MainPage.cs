namespace Pages
{
    using OpenQA.Selenium;
    using WDriver;

    public class MainPage : BasePage
    {
        private static readonly BaseElement Copyright = new BaseElement(By.ClassName("text-right"));

        internal MainPage() : base(MainPage.Copyright.Locator, "Main Page")
        {
        }
    }
}