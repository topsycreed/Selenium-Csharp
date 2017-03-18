using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Selenium_csharp.Properties;
using System;

namespace Selenium_csharp
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            var ChromeDriverLocation = AppDomain.CurrentDomain.BaseDirectory;

            driver = new ChromeDriver(ChromeDriverLocation);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Login()
        {
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("p@ssw0rd");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='header']/a[@href='http://localhost/litecart/admin/logout.php']")));
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
