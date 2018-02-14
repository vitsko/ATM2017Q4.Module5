namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;
    using Utility;
    using WDriver;
    using static Entities.PressRelease;

    [Binding]
    public class TestPressReleasesFeatureSteps : BaseTest
    {
        private static PageOfPressReleases page;
        private static string message;

        [Given(@"I go to page with list of Press Releases")]
        public void GivenIGoToPageWithListOfPressReleases()
        {
            if (!SitePages.IsOpen)
            {
                page = (PageOfPressReleases)SitePages.NavigatePageTo(
                                                                     PageOfPressReleases.MenuPressCenter,
                                                                     PageOfPressReleases.MenuPressReleases,
                                                                     SitePages.Pages.PageOfPressReleases);
            }
        }

        [When(@"Click to button '(.*)'")]
        public void WhenClickToButton(string p0)
        {
            page.ClickLoadMore();
        }

        [Then(@"The amount of press-releases on list less than max amount")]
        public void ThenTheAmountOfPressReleasesOnListLessThanMaxAmount()
        {
            var count = page.CountOfPressReleasesOnList();

            Assert.IsTrue(
                        count <= Config.MaxCountOfPressReleasesOnPage,
                        Resource.CountOfPressReleasesAfterClickMore,
                        count,
                        Config.MaxCountOfPressReleasesOnPage);
        }

        [When(@"I get press-releases with title on the list")]
        public void WhenIGetPress_ReleasesWithTitle()
        {
            ScenarioContext.Current["PressReleasesWithTitle"] = page.PressReleasesWithTitle();
        }

        [Then(@"Title is not null for each press-releases")]
        public void ThenICheckTitleIsNotNullForEachPress_Releases()
        {
            foreach (var pressRelease in ScenarioContext.Current["PressReleasesWithTitle"] as List<PressRelease>)
            {
                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.TitleOfPressReleaseIsNotNull,
                                                                        pressRelease.Id,
                                                                        pressRelease.Title.ElementAt(0),
                                                                        pressRelease.Title.ElementAt(1));

                SoftAssert.That(
                                 pressRelease.Title.All(
                                                 title => !string.IsNullOrWhiteSpace(title)),
                                                 TestPressReleasesFeatureSteps.message);
            }
        }

        [When(@"I get press-releases with link to image")]
        public void WhenIGetPress_ReleasesWithLinkToImage()
        {
            ScenarioContext.Current["PressReleasesWithLinkToImage"] = page.PressReleasesWithSizeOfElement(PageOfPressReleases.XpathToImageOfAnnouncement, PageOfPressReleases.AttributeSrc, SizeOfFile.Image);
        }

        [Then(@"Link to image for each press-releases is correct")]
        public void ThenICheckLinkToImageForEachPress_Releases()
        {
            foreach (var pressRelease in ScenarioContext.Current["PressReleasesWithLinkToImage"] as List<PressRelease>)
            {
                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.CorrectLinkToImageOfPressReleases,
                                                                        pressRelease.Id);

                SoftAssert.That(pressRelease.SizeOfImageByAnnouncement != 0, TestPressReleasesFeatureSteps.message);
            }
        }

        [When(@"I get press-releases with announcement")]
        public void WhenIGetPress_ReleasesWithAnnouncement()
        {
            ScenarioContext.Current["PressReleasesWithAnnouncement"] = page.PressReleasesWithAnnouncements();
        }

        [Then(@"Announcement is not null for each press-releases")]
        public void ThenICheckAnnouncementIsNotNullForEachPress_Releases()
        {
            foreach (var pressRelease in ScenarioContext.Current["PressReleasesWithAnnouncement"] as List<PressRelease>)
            {
                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.AnnouncementOfPressReleaseIsNotNull,
                                                                        pressRelease.Id);

                SoftAssert.That(
                                !string.IsNullOrEmpty(pressRelease.Announcement),
                                TestPressReleasesFeatureSteps.message);
            }
        }

        [When(@"I get press-releases with link to watch PDF of press-releases")]
        public void WhenIGetPress_ReleasesWithLinkToWatchPDFOfPress_Releases()
        {
            ScenarioContext.Current["PressReleasesWithLinkToWatchPDF"] = page.PressReleasesWithSizeOfElement(PageOfPressReleases.XpathToLinkToWatchPDF, PageOfPressReleases.AttributeHref, SizeOfFile.WatchPDF);
        }

        [Then(@"Link to watch PDF of press-release is correct")]
        public void ThenICheckLinkToWatchPDFOfPress_Releases()
        {
            foreach (var pressRelease in ScenarioContext.Current["PressReleasesWithLinkToWatchPDF"] as List<PressRelease>)
            {
                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.CorrectLinkToWatchPDFOfPressReleases,
                                                                        pressRelease.Id);

                SoftAssert.That(pressRelease.SizeOfFileToWatchPDF != 0, TestPressReleasesFeatureSteps.message);
            }
        }

        [When(@"I get press-releases with link to download PDF of press-releases")]
        public void WhenIGetPress_ReleasesWithLinkToDownloadPDFOfPress_Releases()
        {
            ScenarioContext.Current["PressReleasesWithLinkToDownloadPDF"] = page.PressReleasesWithSizeOfElement(PageOfPressReleases.XpathLinkToDownloadPDF, PageOfPressReleases.AttributeHref, SizeOfFile.DownloadPDF);
        }

        [Then(@"Link to download PDF of press-release is correct")]
        public void ThenICheckLinkToDownloadPDFOfPress_Releases()
        {
            foreach (var pressRelease in ScenarioContext.Current["PressReleasesWithLinkToDownloadPDF"] as List<PressRelease>)
            {
                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.CorrectLinkToDownloadPDFOfPressReleases,
                                                                        pressRelease.Id);

                SoftAssert.That(pressRelease.SizeOfFileToDownloadPDF != 0, TestPressReleasesFeatureSteps.message);
            }
        }

        [When(@"I get title of press-release on the page")]
        public void WhenIGetTitleOfPress_ReleaseOnThePage()
        {
            var elementsWithLink = page.GetElementsOfLinkToPageOfPressRelease();

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

            ScenarioContext.Current["TitlesOfPressReleasesOnPage"] = titlesOfPressReleasesOnPage;
        }

        [Then(@"Title of press-release on the list matches up title on the page of press-pelease")]
        public void ThenIMatchTitleOfPress_ReleaseOnTheListAndOnThePageOfPress_Pelease()
        {
            var pressReleases = ScenarioContext.Current["PressReleasesWithTitle"] as List<PressRelease>;
            var titlesOfPressReleasesOnPage = ScenarioContext.Current["TitlesOfPressReleasesOnPage"] as List<string>;

            for (int i = 0; i < pressReleases.Count; i++)
            {
                var titlesOfPressReleasesFromList = pressReleases.ElementAt(i).Title.ToList();
                Helper.JoinStringsInListByPair(titlesOfPressReleasesFromList);
                var pressReleaseOnPage = titlesOfPressReleasesOnPage.ElementAt(i);

                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.MatchTitleOfPressReleasesOnListAndPage,
                                                                        pressReleases.ElementAt(i).Id);

                SoftAssert.That(
                                titlesOfPressReleasesFromList.ElementAt(0).Equals(pressReleaseOnPage, System.StringComparison.InvariantCultureIgnoreCase),
                                TestPressReleasesFeatureSteps.message);
            }
        }

        [When(@"I have filtered press-releases by date start (.*) and date end (.*)")]
        public void WhenIHaveFilteredPressReleasesByDate(string dateFrom, string dateTo)
        {
            ScenarioContext.Current["PressReleasesAfterFiltering"] = page.FilterPressReleasesByDate(dateFrom, dateTo);
            ScenarioContext.Current["dateFrom"] = dateFrom;
            ScenarioContext.Current["dateTo"] = dateTo;
        }

        [Then(@"Date of press-release on the list matches up range of date into filter")]
        public void ThenDateOfPress_ReleaseOnTheListMatchesUpRangeOfDateIntoFilter()
        {
            foreach (var pressRelease in ScenarioContext.Current["PressReleasesAfterFiltering"] as List<PressRelease>)
            {
                TestPressReleasesFeatureSteps.message = string.Format(
                                                                        Resource.CorrectFilteringByDateOfPressReleases,
                                                                        pressRelease.Id,
                                                                        pressRelease.Date.ToString(Config.PatternDate),
                                                                        ScenarioContext.Current["dateFrom"],
                                                                        ScenarioContext.Current["dateTo"]);

                DateTime dateFrom, dateTo;
                Helper.GetDateOnStringByCulture(ScenarioContext.Current["dateFrom"].ToString(), out dateFrom);
                Helper.GetDateOnStringByCulture(ScenarioContext.Current["dateTo"].ToString(), out dateTo);

                SoftAssert.That(
                                pressRelease.Date >= dateFrom
                                && pressRelease.Date <= dateTo,
                                TestPressReleasesFeatureSteps.message);
            }
        }
    }
}