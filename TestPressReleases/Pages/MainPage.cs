namespace TestPressReleases.Pages
{
    using OpenQA.Selenium;
    using WebDriver;

    internal class MainPage : BasePage
    {
        private static readonly BaseElement Copyright = new BaseElement(By.ClassName("text-right"));
        private static readonly BaseElement MenuPressCenter = new BaseElement(By.XPath("//li[@id='435aa1d6-2ddd-43b5-9564-3a986dd3d526']"));
        private static readonly BaseElement MenuPressReleases = new BaseElement(By.XPath(string.Format("//a[text()='{0}']", Config.MenuPressReleases)));

        internal MainPage() : base(MainPage.Copyright.Locator, "Main Page")
        {
        }

        internal ListOfPR GoToPageOfPressReleases()
        {
            WebDriver.HoverOnElement(MainPage.MenuPressCenter);
            MainPage.MenuPressReleases.Click();

            return new ListOfPR();
        }
    }
}