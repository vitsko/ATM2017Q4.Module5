namespace TestPressReleases.Tests
{
    using NUnit.Framework;
    using Pages;
    using WebDriver;

    [TestFixture]
    internal class TestContentBlockOfPressReleases : BaseTest
    {
        [Test, Category("VisibilityOfElements")]
        public void ListOfPressReleasesIsVisible()
        {
            var pageOfPressReleases = ListOfPR.GoToPage();

            Assert.IsTrue(pageOfPressReleases.PressReleasesIsVisible());
        }

        [Test, Category("CountElements")]
        public void DefaultCountOfPressReleases()
        {
            var pageOfList = ListOfPR.GoToPage();
            var defaultCount = pageOfList.CountOfPressReleases();

            Assert.AreEqual(defaultCount, Config.DefaultCountOfPressReleases);
        }

        [Test, Category("CountElements")]
        public void CountOfPressReleasesAfterClickMore()
        {
            var pageListOfPR = ListOfPR.GoToPage();

            pageListOfPR.ClickLoadMore();

            var count = pageListOfPR.CountOfPressReleases();

            Assert.IsTrue(count <= Config.MaxCountOfPROnPage);
        }

        [Test, Category("ObligatoryData")]
        public void TitleOfPressReleaseIsNotNull()
        {
            var page = ListOfPR.GoToPage();

            var dataOfTitles = page.WatchTitlesOfPressReleases();

            Assert.IsTrue(dataOfTitles.TrueForAll(value => !string.IsNullOrWhiteSpace(value))
                          && dataOfTitles.Count / 2 <= Config.MaxCountOfPROnPage);
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToImageOfPressReleases()
        {
            var page = ListOfPR.GoToPage();

            Assert.IsTrue(page.IsCorrectLinkToImageOfPressReleases());
        }

        [Test, Category("ObligatoryData")]
        public void AnnouncementOfPressReleaseIsNotNull()
        {
            var page = ListOfPR.GoToPage();

            var announcement = page.WatchAnnouncementsOfPressReleases();

            Assert.IsTrue(announcement.TrueForAll(value => !string.IsNullOrWhiteSpace(value))
                          && announcement.Count / 2 <= Config.MaxCountOfPROnPage);
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToWatchPDFOfPressReleases()
        {
            var page = ListOfPR.GoToPage();

            Assert.IsTrue(page.IsCorrectLinkToWatchPDFOfPressReleases());
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToDownloadPDFOfPressReleases()
        {
            var page = ListOfPR.GoToPage();

            Assert.IsTrue(page.IsCorrectLinkToDownloadPDFOfPressReleases());
        }

        [Test, Category("ObligatoryData")]
        public void MatchTitleOfPressReleasesOnListAndPage()
        {
            var page = ListOfPR.GoToPage();

            Assert.IsTrue(page.IsCorrectTitlesOnListAndPageOfPressRelease());
        }

        [Test, Category("CountElements")]
        public void CorrectFilteringByDateOfPressReleases()
        {
            var page = ListOfPR.GoToPage();

            var afterFiltering = page.FilterByDate();

            Assert.IsTrue(afterFiltering.MatchDateOfResultFilteringWithDateOfFilters());
        }
    }
}