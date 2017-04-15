using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Selenium_csharp.WrapperFactory;

namespace Selenium_csharp.PageObjects
{
    public class ProductPage
    {
        [FindsBy(How = How.XPath, Using = "//select[@name = 'options[Size]']")]
        public IWebElement size;

        [FindsBy(How = How.XPath, Using = "//button[@name = 'add_cart_product']")]
        public IWebElement addToCart;

        [FindsBy(How = How.XPath, Using = "//div[@class = 'content']//input[@name = 'quantity']")]
        public IWebElement productQuantityElement;

        [FindsBy(How = How.XPath, Using = "//span[@class = 'quantity']")]
        public IWebElement cart;

        bool AreElementsPresent(IWebDriver driver, By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        internal void AddToCart()
        {
            if (AreElementsPresent(WebDriverFactory.Driver, By.XPath("//select[@name = 'options[Size]']")))
            {
                SelectElement selectSize = new SelectElement(size);
                selectSize.SelectByText("Small");
            }

            //Quantity of adding product (by default = 1)
            int productQuantity = int.Parse(productQuantityElement.GetAttribute("valueAsNumber"));

            //Quantity of already added product in Cart
            int cartQuantity = int.Parse(cart.GetAttribute("innerText"));

            addToCart.Click();

            TestBase.wait.Until(e => int.Parse(e.FindElement(By.XPath("//span[@class = 'quantity']")).GetAttribute("innerText")).Equals(cartQuantity + productQuantity));
        }
    }
}
