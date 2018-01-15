namespace TestPressReleases.WebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using OpenQA.Selenium;

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

        internal By Locator { get; private set; }

        protected string Name { get; set; }

        protected IWebElement Element { get; set; }

        public string GetText()
        {
           WebDriver.WaitForIsVisible(this.Locator);
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

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            WebDriver.GetDriver().FindElement(this.Locator).SendKeys(text);
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
           WebDriver.WaitForIsVisible(this.Locator);
            WebDriver.GetDriver().FindElement(this.Locator).Click();
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
           WebDriver.WaitForIsVisible(this.Locator);
            return this.FindElement(this.Locator).Displayed;
        }

        internal List<string> GetValueBySpecialFuncSelector(Func<IWebElement, string> selector)
        {
           WebDriver.WaitForIsVisible(this.Locator);

            return this.FindElements(this.Locator)
                       .ToList()
                       .Select<IWebElement, string>(selector)
                       .ToList();
        }
    }
}