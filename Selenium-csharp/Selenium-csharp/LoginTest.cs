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
            var ChromeDriverLocation = AppDomain.CurrentDomain.BaseDirectory;

            driver = new ChromeDriver(ChromeDriverLocation);
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
                ReadOnlyCollection<IWebElement> PagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/a/span[@class = 'name']"));

                int PageCount = PagesToClick.Count();

                for (int i = 0; i <= PageCount - 1; i++)
                {
                    PagesToClick[i].Click();
                    Assert.That(driver.FindElement(By.XPath("//h1")).Text, Is.Not.Empty);

                    if (AreElementsPresent(driver, By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']")))
                    {
                        ReadOnlyCollection<IWebElement> SubPagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']"));

                        int SubPageCount = SubPagesToClick.Count();

                        for (int j = 0; j <= SubPageCount - 1; j++)
                        {
                            SubPagesToClick[j].Click();
                            Assert.That(driver.FindElement(By.XPath("//h1")).Text, Is.Not.Empty);
                            SubPagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']"));
                        }
                    }

                    PagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/a/span[@class = 'name']"));
                }
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
