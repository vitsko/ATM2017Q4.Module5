namespace TestPressReleases.Tests
{
    using NUnit.Framework;
    using Pages;
    using WebDriver;

    [TestFixture]
    internal class TestContentBlockOfPressReleases : BaseTest
    {
        private static PageOfPressReleases ListOfPressReleases => (PageOfPressReleases)SitePages.NavigatePageTo(PageOfPressReleases.MenuPressCenter, PageOfPressReleases.MenuPressReleases, SitePages.Pages.PageOfPressReleases);

        [Test, Category("VisibilityOfElements")]
        public void ListOfPressReleasesIsVisible()
        {
            Assert.IsTrue(ListOfPressReleases.PressReleasesIsVisible());
        }

        [Test, Category("CountElements")]
        public void DefaultCountOfPressReleases()
        {
            var defaultCount = ListOfPressReleases.CountOfPressReleases();

            Assert.AreEqual(defaultCount, Config.DefaultCountOfPressReleases);
        }

        [Test, Category("CountElements")]
        public void CountOfPressReleasesAfterClickMore()
        {
            var page = ListOfPressReleases.ClickLoadMore();

            var count = page.CountOfPressReleases();

            Assert.IsTrue(count <= Config.MaxCountOfPressReleasesOnPage);
        }

        [Test, Category("ObligatoryData")]
        public void TitleOfPressReleaseIsNotNull()
        {
            var dataOfTitles = ListOfPressReleases.WatchTitlesOfPressReleases();

            Assert.IsTrue(dataOfTitles.TrueForAll(value => !string.IsNullOrWhiteSpace(value))
                          && dataOfTitles.Count / 2 <= Config.MaxCountOfPressReleasesOnPage);
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToImageOfPressReleases()
        {
            Assert.IsTrue(ListOfPressReleases.IsCorrectLinkToImageOfPressReleases());
        }

        [Test, Category("ObligatoryData")]
        public void AnnouncementOfPressReleaseIsNotNull()
        {
            var announcement = ListOfPressReleases.WatchAnnouncementsOfPressReleases();

            Assert.IsTrue(announcement.TrueForAll(value => !string.IsNullOrWhiteSpace(value))
                          && announcement.Count / 2 <= Config.MaxCountOfPressReleasesOnPage);
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToWatchPDFOfPressReleases()
        {
            Assert.IsTrue(ListOfPressReleases.IsCorrectLinkToWatchPDFOfPressReleases());
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToDownloadPDFOfPressReleases()
        {
            Assert.IsTrue(ListOfPressReleases.IsCorrectLinkToDownloadPDFOfPressReleases());
        }

        [Test, Category("ObligatoryData")]
        public void MatchTitleOfPressReleasesOnListAndPage()
        {
            Assert.IsTrue(ListOfPressReleases.IsCorrectTitlesOnListAndPageOfPressRelease());
        }

        [Test, Category("CountElements")]
        public void CorrectFilteringByDateOfPressReleases()
        {
            var afterFiltering = ListOfPressReleases.FilterByDate();

            Assert.IsTrue(afterFiltering.MatchDateOfResultFilteringWithDateOfFilters());
        }
    }
}