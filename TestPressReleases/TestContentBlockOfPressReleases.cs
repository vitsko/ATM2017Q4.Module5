namespace Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Logger;
    using NUnit.Framework;
    using Pages;
    using Serilog;
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

                Log.Information(string.Format(Resource.OpenPage, WDriver.GetDriver().Url));

                return page;
            }
        }

        [Test, Category("CountElements")]
        public void DefaultCountOfPressReleases()
        {
            var pressReleases = ListOfPressReleases.PressReleasesToCount();

            TestContentBlockOfPressReleases.message = string.Format(
                                                                     Resource.DefaultCountOfPressReleases,
                                                                     pressReleases,
                                                                     Config.DefaultCountOfPressReleases);

            Logger.MessageAboutError = TestContentBlockOfPressReleases.message;

            Assert.IsTrue(pressReleases.Count != Config.DefaultCountOfPressReleases, TestContentBlockOfPressReleases.message);
        }

        [Test, Category("CountElements")]
        public void CountOfPressReleasesAfterClickMore()
        {
            var page = ListOfPressReleases.ClickLoadMore();

            var pressReleases = page.PressReleasesToCount();

            TestContentBlockOfPressReleases.message = string.Format(
                                                                    Resource.CountOfPressReleasesAfterClickMore,
                                                                    pressReleases.Count,
                                                                    Config.MaxCountOfPressReleasesOnPage);

            Logger.MessageAboutError = TestContentBlockOfPressReleases.message;

            Assert.IsTrue(
                          pressReleases.Count <= Config.MaxCountOfPressReleasesOnPage, TestContentBlockOfPressReleases.message);
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

                var result = pressRelease.Title.All(title => !string.IsNullOrWhiteSpace(title));

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
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

                var result = pressRelease.SizeOfImageByAnnouncement != 0;

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
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

                var result = !string.IsNullOrEmpty(pressRelease.Announcement);

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
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

                var result = pressRelease.SizeOfFileToWatchPDF != 0;

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
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

                var result = pressRelease.SizeOfFileToDownloadPDF != 0;

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
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

                var result = titlesOfPressReleasesFromList.ElementAt(0).Equals(pressReleaseOnPage, System.StringComparison.InvariantCultureIgnoreCase);

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
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

                var result = pressRelease.Date >= ListOfPressReleases.DateFrom && pressRelease.Date <= ListOfPressReleases.DateTo;

                if (!result)
                {
                    this.WriteToLogFailedSoftAssertsMessage(TestContentBlockOfPressReleases.message);
                }

                SoftAssert.That(result, TestContentBlockOfPressReleases.message);
            }
        }
    }
}