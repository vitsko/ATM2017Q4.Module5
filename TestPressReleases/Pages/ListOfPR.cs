namespace TestPressReleases.Pages
{
    using OpenQA.Selenium;
    using WebDriver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using System.Configuration;
    using System.Net;

    internal class ListOfPR : BasePage
    {
        private static readonly int countOfPressReleases = int.Parse(ConfigurationManager.AppSettings["CountOfPressReleases"]);
        private static readonly int countOfClickMoreLoad = int.Parse(ConfigurationManager.AppSettings["CountOfClickMoreLoad"]);

        private static readonly BaseElement pressReleases = new BaseElement(By.ClassName("pressrelease-list-widget"));
        private static readonly BaseElement loadMore = new BaseElement(By.XPath("//button[@data-bind='visible: CanLoadMore']"));
        private static readonly BaseElement listOfPR = new BaseElement(By.XPath("//div[@role='tablist']"));

        private static readonly BaseElement titleOfPressReleases = new BaseElement(By.XPath("//div[@class='panel-heading']//span"));
        private static readonly BaseElement linksOfImage = new BaseElement(By.XPath("//div[@class='panel-body']//div[@class='image']//img"));
        private static readonly string attributeOfImg = "src";

        private static readonly BaseElement announcement = new BaseElement(By.XPath("//div[@class='tableOverflow']"));

        private static readonly BaseElement linkToWatchPDF = new BaseElement(By.XPath("//a[contains(@class,'icon-s-flipbook-pdf_round')]"));
        private static readonly BaseElement linkToDownloadPDF = new BaseElement(By.XPath("//a[contains(@class,'icon-s-download_round')]"));
        private static readonly string attributeOfPDF = "href";

        private static readonly int maxCountOfPROnPage = (ListOfPR.countOfClickMoreLoad + 1) * ListOfPR.countOfPressReleases;

        public ListOfPR() : base(pressReleases.Locator, "Press Releases")
        {

        }

        internal ListOfPR ClickLoadMore()
        {
            Assert.IsTrue(ListOfPR.pressReleases.IsVisible());//this.BlockIsVisible());
            Assert.AreEqual(this.DefaultCountOfPressReleases(), ListOfPR.countOfPressReleases);

            this.ClickToLoadMore();

            Assert.IsTrue(ListOfPR.listOfPR.FindElements(ListOfPR.listOfPR.Locator).Count <= ListOfPR.maxCountOfPROnPage);

            return this;
        }

        internal ListOfPR WatchInfoOfPressReleases()
        {
            var titles = ListOfPR.titleOfPressReleases.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);

            Assert.IsTrue(titles.TrueForAll(value => !string.IsNullOrWhiteSpace(value))
                          && titles.Count / 2 <= ListOfPR.maxCountOfPROnPage);

            Assert.IsTrue(CheckLinkOfFile(ListOfPR.linksOfImage, ListOfPR.attributeOfImg));

            var announcement = ListOfPR.announcement.GetValueBySpecialFuncSelector(iwebElement => iwebElement.Text);

            Assert.IsTrue(announcement.TrueForAll(value => !string.IsNullOrWhiteSpace(value))
                          && announcement.Count / 2 <= ListOfPR.maxCountOfPROnPage);


            Assert.IsTrue(CheckLinkOfFile(ListOfPR.linkToWatchPDF, ListOfPR.attributeOfPDF));
            Assert.IsTrue(CheckLinkOfFile(ListOfPR.linkToDownloadPDF, ListOfPR.attributeOfPDF));

            return this;
        }

        internal PageOfPR WatchTitleOfPRonPage()
        {

            return new PageOfPR();
        }

        internal ListOfPR WorkFilterByDate()
        {
            return this;
        }

        private int DefaultCountOfPressReleases()
        {
            return ListOfPR.pressReleases.FindElements(listOfPR.Locator).Count;
        }

        private void ClickToLoadMore()
        {
            try
            {
                for (int i = 0; i <= ListOfPR.countOfClickMoreLoad - 1; i++)
                {
                    ListOfPR.loadMore.Click();
                }
            }
            catch
            {
                throw;
            }
        }

        private bool CheckLinkOfFile(BaseElement element, string attribute)
        {
            element.WaitForIsVisible();

            var urls = element.GetValueBySpecialFuncSelector(iwebElement => iwebElement.GetAttribute(attribute));

            WebRequest request;
            HttpWebResponse response;

            var sizes = new List<long>();

            for (int i = 0; i < urls.Count; i++)
            {
                request = WebRequest.Create(urls.ElementAt(i));
                response = (HttpWebResponse)request.GetResponse();

                sizes.Add(response.ContentLength);
            }

            return sizes.TrueForAll(size => size > 0);
        }
    }
}