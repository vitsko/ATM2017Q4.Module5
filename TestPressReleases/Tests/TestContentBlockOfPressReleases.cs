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
                    page = (PageOfPressReleases)SitePages.NavigatePageTo(
                                                                         PageOfPressReleases.MenuPressCenter,
                                                                         PageOfPressReleases.MenuPressReleases,
                                                                         SitePages.Pages.PageOfPressReleases);
                }

                return page;
            }
        }

        [Test, Category("CountElements")]
        public void DefaultCountOfPressReleases()
        {
            var defaultCount = ListOfPressReleases.CountOfPressReleases();

            Assert.AreEqual(
                            defaultCount,
                            Config.DefaultCountOfPressReleases,
                            Resource.DefaultCountOfPressReleases,
                            defaultCount,
                            Config.DefaultCountOfPressReleases);
        }

        [Test, Category("CountElements")]
        public void CountOfPressReleasesAfterClickMore()
        {
            var page = ListOfPressReleases.ClickLoadMore();

            var count = page.CountOfPressReleases();

            Assert.IsTrue(
                          count <= Config.MaxCountOfPressReleasesOnPage,
                          Resource.CountOfPressReleasesAfterClickMore,
                          count,
                          Config.MaxCountOfPressReleasesOnPage);
        }

        [Test, Category("ObligatoryData")]
        public void TitleOfPressReleaseIsNotNull()
        {
            var idAndTitleOfPressReleases = ListOfPressReleases.GetIdAndTitleOfPressReleases();

            foreach (var title in idAndTitleOfPressReleases)
            {
                Assert.That(
                            title.Value.All(
                                            data => !string.IsNullOrWhiteSpace(data)),
                                           Resource.TitleOfPressReleaseIsNotNull,
                                            title.Key,
                                            title.Value.ElementAt(0),
                                            title.Value.ElementAt(1));
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToImageOfPressReleases()
        {
            var idAndSizeOfImageForPressRelease = ListOfPressReleases.GetSizeOfElementForPressReleases(PageOfPressReleases.XpathToImageOfAnnouncement, PageOfPressReleases.AttributeSrc);

            foreach (var item in idAndSizeOfImageForPressRelease)
            {
                Assert.That(
                            item.Value != 0,
                            Resource.CorrectLinkToImageOfPressReleases,
                            item.Key);
            }
        }

        [Test, Category("ObligatoryData")]
        public void AnnouncementOfPressReleaseIsNotNull()
        {
            var idAndAnnouncements = ListOfPressReleases.GetAnnouncementsOfPressReleases();

            foreach (var item in idAndAnnouncements)
            {
                Assert.That(
                            !string.IsNullOrEmpty(item.Value),
                            Resource.AnnouncementOfPressReleaseIsNotNull,
                            item.Key);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToWatchPDFOfPressReleases()
        {
            var idAndSizeOfPDFForPressRelease = ListOfPressReleases.GetSizeOfElementForPressReleases(PageOfPressReleases.XpathToLinkToWatchPDF, PageOfPressReleases.AttributeHref);

            foreach (var item in idAndSizeOfPDFForPressRelease)
            {
                Assert.That(
                            item.Value != 0,
                            Resource.CorrectLinkToWatchPDFOfPressReleases,
                            item.Key);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToDownloadPDFOfPressReleases()
        {
            var idAndSizeOfPDFForPressRelease = ListOfPressReleases.GetSizeOfElementForPressReleases(PageOfPressReleases.XpathLinkToDownloadPDF, PageOfPressReleases.AttributeHref);

            foreach (var item in idAndSizeOfPDFForPressRelease)
            {
                Assert.That(
                            item.Value != 0,
                            Resource.CorrectLinkToDownloadPDFOfPressReleases,
                            item.Key);
            }
        }

        [Test, Category("ObligatoryData")]
        public void MatchTitleOfPressReleasesOnListAndPage()
        {
            var idAndTitlesOfPressReleasesFromList = ListOfPressReleases.GetIdAndTitleOfPressReleases();
            var elementsWithLink = ListOfPressReleases.GetElementsOfLinkToPageOfPressRelease();
            var titlesOfPressReleasesOnPage = PageOfPressRelease.GetTitlesOfPressRelease(elementsWithLink);

            for (int i = 0; i < idAndTitlesOfPressReleasesFromList.Count; i++)
            {
                var titlesOfPressReleasesFromList = idAndTitlesOfPressReleasesFromList.ElementAt(i).Value.ToList();
                Helper.JoinStringsInListByPair(titlesOfPressReleasesFromList);
                var pressReleaseOnPage = titlesOfPressReleasesOnPage.ElementAt(i);

                StringAssert.AreEqualIgnoringCase(
                                                  titlesOfPressReleasesFromList.ElementAt(0),
                                                  pressReleaseOnPage,
                                                  Resource.MatchTitleOfPressReleasesOnListAndPage,
                                                  idAndTitlesOfPressReleasesFromList.ElementAt(i).Key);
            }
        }

        [Test, Category("CountElements")]
        public void CorrectFilteringByDateOfPressReleases()
        {
            var afterFiltering = ListOfPressReleases.FilterPressReleasesByDate();

            foreach (var item in afterFiltering)
            {
                Assert.That(
                            item.Value >= ListOfPressReleases.DateFrom && item.Value <= ListOfPressReleases.DateTo,
                            Resource.CorrectFilteringByDateOfPressReleases,
                            item.Key,
                            item.Value,
                            ListOfPressReleases.DateFrom.ToString(ListOfPressReleases.PatternDate),
                            ListOfPressReleases.DateTo.ToString(ListOfPressReleases.PatternDate));
            }
        }
    }
}