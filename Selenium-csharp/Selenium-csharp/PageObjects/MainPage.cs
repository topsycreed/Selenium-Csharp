using Selenium_csharp.WrapperFactory;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace Selenium_csharp.PageObjects
{
    public class MainPage
    {
        [FindsBy(How = How.XPath, Using = "//a[@class = 'link']/div[@class='name']/..")]
        public IWebElement product;

        internal void NavigateTo()
        {
            WebDriverFactory.Driver.Navigate().GoToUrl("http://localhost/litecart/en/");
        }

        internal void OpenFirstProduct()
        {
            product.Click();
        }
    }
}
