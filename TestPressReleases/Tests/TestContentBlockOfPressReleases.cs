namespace TestPressReleases.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using WebDriver;

    [TestFixture]
    internal class TestContentBlockOfPressReleases : BaseTest
    {
        private static PageOfPressReleases page;

        private static PageOfPressReleases ListOfPressReleases
        {
            get
            {
                if (!SitePages.IsOpen)
                {
                    page = (PageOfPressReleases)SitePages.NavigatePageTo(PageOfPressReleases.MenuPressCenter, PageOfPressReleases.MenuPressReleases, SitePages.Pages.PageOfPressReleases);
                }

                return page;
            }
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

            Assert.IsTrue(dataOfTitles.TrueForAll(value => !string.IsNullOrWhiteSpace(value)));
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToImageOfPressReleases()
        {
            var titleAndSizeOfImageForPressRelease = ListOfPressReleases.GetSizeOfElementForPressReleases(PageOfPressReleases.XpathToImageOfAnnouncement, PageOfPressReleases.AttributeSrc);

            foreach (var item in titleAndSizeOfImageForPressRelease)
            {
                Assert.That(
                            item.Value != 0,
                            "Press-release \"{0}\" has incorrect link to image of announcement",
                            item.Key);
            }
        }

        [Test, Category("ObligatoryData")]
        public void AnnouncementOfPressReleaseIsNotNull()
        {
            var titlesAndAnnouncements = ListOfPressReleases.GetAnnouncementsOfPressReleases();

            foreach (var item in titlesAndAnnouncements)
            {
                Assert.That(
                            !string.IsNullOrEmpty(item.Value),
                            "Press-release \"{0}\" hasn't an announcement",
                            item.Key);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToWatchPDFOfPressReleases()
        {
            var titleAndSizeOfPDFForPressRelease = ListOfPressReleases.GetSizeOfElementForPressReleases(PageOfPressReleases.XpathToLinkToWatchPDF, PageOfPressReleases.AttributeHref);

            foreach (var item in titleAndSizeOfPDFForPressRelease)
            {
                Assert.That(
                            item.Value != 0,
                            "Press-release \"{0}\" has incorrect link to link to watch PDF-file",
                            item.Key);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToDownloadPDFOfPressReleases()
        {
            var titleAndSizeOfPDFForPressRelease = ListOfPressReleases.GetSizeOfElementForPressReleases(PageOfPressReleases.XpathLinkToDownloadPDF, PageOfPressReleases.AttributeHref);

            foreach (var item in titleAndSizeOfPDFForPressRelease)
            {
                Assert.That(
                            item.Value != 0,
                            "Press-release \"{0}\" has incorrect link to download PDF-file",
                            item.Key);
            }
        }

        [Test, Category("ObligatoryData")]
        public void MatchTitleOfPressReleasesOnListAndPage()
        {
            var titlesOfPressReleasesFromList = ListOfPressReleases.WatchTitlesOfPressReleases();
            var elementsWithLink = ListOfPressReleases.GetElementsOfLinkToPageOfPressRelease();
            var titlesOfPressReleasesOnPage = PageOfPressRelease.GetTitlesOfPressRelease(elementsWithLink);

            for (int i = 0; i < titlesOfPressReleasesFromList.Count; i++)
            {
                var pressReleaseFromList = titlesOfPressReleasesFromList.ElementAt(i);
                var pressReleaseOnPage = titlesOfPressReleasesOnPage.ElementAt(i);

                StringAssert.AreEqualIgnoringCase(
                                                  pressReleaseFromList,
                                                  pressReleaseOnPage,
                                                  "Title into list of press-releases \"{0}\" don't match to title on page of press-release \"{1}\".",
                                                  pressReleaseFromList,
                                                  pressReleaseOnPage);
            }
        }

        [Test, Category("CountElements")]
        public void CorrectFilteringByDateOfPressReleases()
        {
            var afterFiltering = ListOfPressReleases.FilterByDate();

            foreach (var item in afterFiltering)
            {
                Assert.That(
                            item.Value >= ListOfPressReleases.DateFrom && item.Value <= ListOfPressReleases.DateTo,
                            "List Of press-releases contains press release \"{0}\" with incorrect date {1}. Correct date is in period [{2} - {3}]",
                            item.Key,
                            item.Value,
                            ListOfPressReleases.DateFrom.ToString(ListOfPressReleases.PatternDate),
                            ListOfPressReleases.DateTo.ToString(ListOfPressReleases.PatternDate));
            }
        }
    }
}