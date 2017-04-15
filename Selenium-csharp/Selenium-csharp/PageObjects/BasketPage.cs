using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Selenium_csharp.PageObjects
{
    public class BasketPage : TestBase
    {
        [FindsBy(How = How.XPath, Using = "//a[@class = 'link' and contains(@href, 'checkout')]")]
        public IWebElement checkout;

        internal void Open()
        {
            checkout.Click();
        }

        [FindsBy(How = How.XPath, Using = "//form[@name = 'cart_form']")]
        public IList<IWebElement> itemsForm;

        [FindsBy(How = How.XPath, Using = "//ul[@class = 'shortcuts']//a")]
        public IList<IWebElement> shortcats;

        internal void RemoveProducts()
        {
            int itemsCount = itemsForm.Count;

            if (itemsCount > 1)
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    IWebElement itemRemoveButton = itemsForm[i].FindElement(By.XPath(".//button[@name = 'remove_cart_item']"));

                    IWebElement itemNameElement = itemsForm[i].FindElement(By.XPath(".//strong"));
                    string itemName = itemNameElement.GetAttribute("innerText");

                    int j = 0;

                    int shortcatsCount = shortcats.Count;

                    //Run through the list of shortcats to fing product with clickable remove
                    while (!itemRemoveButton.Displayed && j < shortcatsCount)
                    {
                        try
                        {
                            shortcats[j].Click();
                            shortWait.Until(ExpectedConditions.ElementToBeClickable(itemRemoveButton));
                        }
                        catch (WebDriverTimeoutException)
                        {
                            j++;
                        }
                    }
                    itemRemoveButton.Click();
                    itemsCount--;

                    //Wait to check that product also deleted from table
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//td[@class = 'item' and text() = '" + itemName + "']")));
                }
            }

            //If finded/left only 1 form - there is no shortcats, just click delete button
            if (itemsCount == 1)
            {
                IWebElement itemRemoveButton = itemsForm[0].FindElement(By.XPath(".//button[@name = 'remove_cart_item']"));

                IWebElement itemNameElement = itemsForm[0].FindElement(By.XPath(".//strong"));
                string itemName = itemNameElement.GetAttribute("innerText");

                itemRemoveButton.Click();

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//td[@class = 'item' and text() = '" + itemName + "']")));
            }
        }

        [FindsBy(How = How.XPath, Using = "//em")]
        private IWebElement _noItemMessage;

        internal string GetNoItemMessageText
        {
            get
            {
                return _noItemMessage.Text;
            }
        }
    }
}
