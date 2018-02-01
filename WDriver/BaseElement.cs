namespace WDriver
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using OpenQA.Selenium;

    public class BaseElement : IWebElement
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

        public By Locator { get; private set; }

        protected string Name { get; set; }

        protected IWebElement Element { get; set; }

        public string GetText()
        {
            WDriver.WaitForIsVisible(this.Locator);
            return this.Element.Text;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            WDriver.GetDriver().FindElement(this.Locator).SendKeys(text);
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            WDriver.WaitForIsVisible(this.Locator);
            WDriver.GetDriver().FindElement(this.Locator).Click();
        }

        public string GetAttribute(string attributeName)
        {
            return WDriver.GetDriver()
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
            return WDriver.GetDriver().FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return WDriver.GetDriver().FindElements(by);
        }

        public bool IsVisible()
        {
            WDriver.WaitForIsVisible(this.Locator);
            return this.FindElement(this.Locator).Displayed;
        }

        public List<string> GetValueBySpecialFuncSelector(Func<IWebElement, string> selector)
        {
            WDriver.WaitForIsVisible(this.Locator);

            return this.FindElements(this.Locator)
                       .ToList()
                       .Select<IWebElement, string>(selector)
                       .ToList();
        }
    }
}