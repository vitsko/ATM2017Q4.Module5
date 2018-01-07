namespace TestPressReleases.WebDriver
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using System.Collections.Generic;
    using System.Linq;

    internal class BaseElement : IWebElement
    {
        public BaseElement(By locator, string name)
        {
            this.Locator = locator;
            this.Name = name.Equals(string.Empty) ? this.GetText() : name;
        }

        public BaseElement(By locator)
        {
            this.Locator = locator;
        }

        public string TagName { get; }

        public string Text { get; }

        public bool Enabled { get; }

        public bool Selected { get; }

        public Point Location { get; }

        public Size Size { get; }

        public bool Displayed { get; }

        protected string Name { get; set; }

        internal By Locator { get; private set; }

        protected IWebElement Element { get; set; }

        public string GetText()
        {
            this.WaitForIsVisible();
            return this.Element.Text;
        }

        public IWebElement GetElement()
        {
            try
            {
                this.Element = WebDriver.GetDriver().FindElement(this.Locator);
            }
            catch (Exception)
            {
                throw;
            }

            return this.Element;
        }

        public void WaitForIsVisible()
        {
            new WebDriverWait(WebDriver.GetDriver(), TimeSpan.FromSeconds(WebDriver.TimeoutForElement)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(this.Locator));
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            throw new NotImplementedException();
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            this.WaitForIsVisible();
            WebDriver.GetDriver().FindElement(this.Locator).Click();
            this.WaitForIsVisible();
        }

        public string GetAttribute(string attributeName)
        {
            return WebDriver.GetDriver()
                            .FindElement(this.Locator)
                            .GetAttribute(attributeName);
        }

        public string GetProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By by)
        {
            return WebDriver.GetDriver().FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return WebDriver.GetDriver().FindElements(by);
        }

        public bool IsVisible()
        {
            this.WaitForIsVisible();
            return this.FindElement(this.Locator).Displayed;
        }

        public List<string> GetValueBySpecialFuncSelector(Func<IWebElement, string> selector)
        {
            this.WaitForIsVisible();

            return this.FindElements(this.Locator)
                       .ToList()
                       .Select<IWebElement, string>(selector)
                       .ToList();
        }
    }
}