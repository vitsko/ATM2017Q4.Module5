namespace Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using Utility;
    using WDriver;
    using static Entities.PressRelease;

    [TestFixture]
    internal class TestContentBlockOfPressReleases : BaseTest
    {
        private static PageOfPressReleases page;
        private static string message;

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
            var pressReleases = ListOfPressReleases.PressReleasesToCount();

            Assert.IsTrue(
                            pressReleases.Count == Config.DefaultCountOfPressReleases,
                            Resource.DefaultCountOfPressReleases,
                            pressReleases,
                            Config.DefaultCountOfPressReleases);
        }

        [Test, Category("CountElements")]
        public void CountOfPressReleasesAfterClickMore()
        {
            var page = ListOfPressReleases.ClickLoadMore();

            var pressReleases = page.PressReleasesToCount();

            Assert.IsTrue(
                          pressReleases.Count <= Config.MaxCountOfPressReleasesOnPage,
                          Resource.CountOfPressReleasesAfterClickMore,
                          pressReleases.Count,
                          Config.MaxCountOfPressReleasesOnPage);
        }

        [Test, Category("ObligatoryData")]
        public void TitleOfPressReleaseIsNotNull()
        {
            var pressReleases = ListOfPressReleases.PressReleasesWithTitle();

            foreach (var pressRelease in pressReleases)
            {
                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.TitleOfPressReleaseIsNotNull,
                                                                        pressRelease.Id,
                                                                        pressRelease.Title.ElementAt(0),
                                                                        pressRelease.Title.ElementAt(1));

                SoftAssert.That(
                                 pressRelease.Title.All(
                                                 title => !string.IsNullOrWhiteSpace(title)),
                                                 TestContentBlockOfPressReleases.message);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToImageOfPressReleases()
        {
            var pressReleases = ListOfPressReleases.PressReleasesWithSizeOfElement(PageOfPressReleases.XpathToImageOfAnnouncement, PageOfPressReleases.AttributeSrc, SizeOfFile.Image);

            foreach (var pressRelease in pressReleases)
            {
                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.CorrectLinkToImageOfPressReleases,
                                                                        pressRelease.Id);

                SoftAssert.That(pressRelease.SizeOfImageByAnnouncement != 0, TestContentBlockOfPressReleases.message);
            }
        }

        [Test, Category("ObligatoryData")]
        public void AnnouncementOfPressReleaseIsNotNull()
        {
            var pressReleases = ListOfPressReleases.PressReleasesWithAnnouncements();

            foreach (var pressRelease in pressReleases)
            {
                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.AnnouncementOfPressReleaseIsNotNull,
                                                                        pressRelease.Id);

                SoftAssert.That(
                                !string.IsNullOrEmpty(pressRelease.Announcement),
                                TestContentBlockOfPressReleases.message);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToWatchPDFOfPressReleases()
        {
            var pressReleases = ListOfPressReleases.PressReleasesWithSizeOfElement(PageOfPressReleases.XpathToLinkToWatchPDF, PageOfPressReleases.AttributeHref, SizeOfFile.WatchPDF);

            foreach (var pressRelease in pressReleases)
            {
                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.CorrectLinkToWatchPDFOfPressReleases,
                                                                        pressRelease.Id);

                SoftAssert.That(pressRelease.SizeOfFileToWatchPDF != 0, TestContentBlockOfPressReleases.message);
            }
        }

        [Test, Category("CorrectLinks")]
        public void CorrectLinkToDownloadPDFOfPressReleases()
        {
            var pressReleases = ListOfPressReleases.PressReleasesWithSizeOfElement(PageOfPressReleases.XpathLinkToDownloadPDF, PageOfPressReleases.AttributeHref, SizeOfFile.DownloadPDF);

            foreach (var pressRelease in pressReleases)
            {
                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.CorrectLinkToDownloadPDFOfPressReleases,
                                                                        pressRelease.Id);

                SoftAssert.That(pressRelease.SizeOfFileToDownloadPDF != 0, TestContentBlockOfPressReleases.message);
            }
        }

        [Test, Category("ObligatoryData")]
        public void MatchTitleOfPressReleasesOnListAndPage()
        {
            var pressReleases = ListOfPressReleases.PressReleasesWithTitle();
            var elementsWithLink = ListOfPressReleases.GetElementsOfLinkToPageOfPressRelease();

            var titlesOfPressReleasesOnPage = new List<string>();

            foreach (var link in elementsWithLink)
            {
                var tab = WDriver.OpenLinkInNewTab(link);
                var pageOfPressRelease = new PageOfPressRelease();
                titlesOfPressReleasesOnPage.AddRange(pageOfPressRelease.GetTitleOfPressReleaseOnPage());

                tab.Close();

                WDriver.GetDriver().SwitchTo().Window(WDriver.GetDriver().WindowHandles.First());
            }

            Helper.PostHandlingForDateOfPressReleases(titlesOfPressReleasesOnPage, true);
            Helper.JoinStringsInListByPair(titlesOfPressReleasesOnPage);

            for (int i = 0; i < pressReleases.Count; i++)
            {
                var titlesOfPressReleasesFromList = pressReleases.ElementAt(i).Title.ToList();
                Helper.JoinStringsInListByPair(titlesOfPressReleasesFromList);
                var pressReleaseOnPage = titlesOfPressReleasesOnPage.ElementAt(i);

                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.MatchTitleOfPressReleasesOnListAndPage,
                                                                        pressReleases.ElementAt(i).Id);

                SoftAssert.That(
                                titlesOfPressReleasesFromList.ElementAt(0).Equals(pressReleaseOnPage, System.StringComparison.InvariantCultureIgnoreCase),
                                TestContentBlockOfPressReleases.message);
            }
        }

        [Test, Category("CountElements")]
        public void CorrectFilteringByDateOfPressReleases()
        {
            var pressReleases = ListOfPressReleases.FilterPressReleasesByDate();

            foreach (var pressRelease in pressReleases)
            {
                TestContentBlockOfPressReleases.message = string.Format(
                                                                        Resource.CorrectFilteringByDateOfPressReleases,
                                                                        pressRelease.Id,
                                                                        pressRelease.Date,
                                                                        ListOfPressReleases.DateFrom.ToString(ListOfPressReleases.PatternDate),
                                                                        ListOfPressReleases.DateTo.ToString(ListOfPressReleases.PatternDate));

                SoftAssert.That(
                            pressRelease.Date >= ListOfPressReleases.DateFrom && pressRelease.Date <= ListOfPressReleases.DateTo,
                            TestContentBlockOfPressReleases.message);
            }
        }
    }
}