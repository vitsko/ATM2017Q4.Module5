namespace Pages
{
    using OpenQA.Selenium;
    using WDriver;

    public class SitePages
    {
        public enum Pages
        {
            PageOfPressReleases
        }

        public static bool IsOpen { get; set; }

        public static BasePage NavigatePageTo(BaseElement pointMenuToHover, BaseElement pointMenuToClick, SitePages.Pages page)
        {
            WDriver.HoverOnElement(pointMenuToHover);

            var elementToClick = pointMenuToClick.FindElement(pointMenuToClick.Locator);
            WDriver.SetBackgroundColorForElement(elementToClick);
            pointMenuToClick.Click();

            return SitePages.GetPage(page);
        }

        private static BasePage GetPage(Pages page)
        {
            SitePages.IsOpen = true;

            switch (page)
            {
                case Pages.PageOfPressReleases:
                    return new PageOfPressReleases();

                default:
                    return new MainPage();
            }
        }
    }
}