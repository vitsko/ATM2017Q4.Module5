namespace TestPressReleases.Pages
{
    using WebDriver;

    internal class SitePages
    {
        internal enum Pages
        {
            PageOfPressReleases
        }

        internal static BasePage NavigatePageTo(BaseElement pointMenuToHover, BaseElement pointMenuToClick, SitePages.Pages page)
        {
            WebDriver.HoverOnElement(pointMenuToHover);
            pointMenuToClick.Click();

            return SitePages.GetPage(page);
        }

        private static BasePage GetPage(Pages page)
        {
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