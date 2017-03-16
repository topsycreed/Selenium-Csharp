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
            driver.Url = Resources.Url;
            driver.FindElement(By.Id("mailbox__login")).SendKeys(Resources.Login);
            driver.FindElement(By.Id("mailbox__login__domain")).SendKeys(Resources.Domain);
            driver.FindElement(By.Id("mailbox__password")).SendKeys(Resources.Password);
            driver.FindElement(By.Id("mailbox__auth__remember__checkbox")).Click();
            driver.FindElement(By.Id("mailbox__auth__button")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("PH_user-email")));

            string correctEmail = "JohnDoe1990@list.ru".ToLower();
            string email = driver.FindElement(By.Id("PH_user-email")).Text;

            Assert.That(correctEmail, Is.EqualTo(email));
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
