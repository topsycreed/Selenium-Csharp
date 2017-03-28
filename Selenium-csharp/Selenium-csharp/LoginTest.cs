using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Selenium_csharp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium_csharp
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        bool AreElementsPresent(IWebDriver driver, By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        [SetUp]
        public void start()
        {
            var chromeDriverLocation = AppDomain.CurrentDomain.BaseDirectory;

            driver = new ChromeDriver(chromeDriverLocation);
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Login()
        {
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("p@ssw0rd");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@title='Logout']")));
        }

        [Test]
        public void CheckAllPages()
        {
            Login();

            if (AreElementsPresent(driver, By.XPath("//ul[@id = 'box-apps-menu']/li/a/span[@class = 'name']")))
            {
                ReadOnlyCollection<IWebElement> pagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/a/span[@class = 'name']"));

                int pageCount = pagesToClick.Count();

                for (int i = 0; i < pageCount; i++)
                {
                    pagesToClick[i].Click();
                    Assert.That(driver.FindElement(By.XPath("//h1")).Text, Is.Not.Empty);

                    if (AreElementsPresent(driver, By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']")))
                    {
                        ReadOnlyCollection<IWebElement> subPagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']"));

                        int subPageCount = subPagesToClick.Count();

                        for (int j = 0; j <= subPageCount - 1; j++)
                        {
                            subPagesToClick[j].Click();
                            Assert.That(driver.FindElement(By.XPath("//h1")).Text, Is.Not.Empty);
                            subPagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']"));
                        }
                    }

                    pagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/a/span[@class = 'name']"));
                }
            }
        }

        [Test]
        public void CheckSticker()
        {
            driver.Url = "http://localhost/litecart/en/";

            ReadOnlyCollection<IWebElement> products = driver.FindElements(By.XPath("//a[@class = 'link']/div[@class='name']/.."));

            int productCount = products.Count();

            for (int i = 0; i < productCount; i++)
            {
                var product = products[i];
                ReadOnlyCollection<IWebElement> stickers = product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]"));

                Assert.That(stickers.Count(), Is.EqualTo(1));
            }
        }

        [TearDown]
        public void Stop()
        {
            if (driver != null)
                driver.Quit();
            driver = null;
        }
    }
}
