using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium_csharp.WrapperFactory;
using System;

namespace Selenium_csharp
{
    public class TestBase
    {
        public static IWebDriver driver;
        public static WebDriverWait wait;
        public static WebDriverWait shortWait;

        [SetUp]
        public void start()
        {
            string browser = "Chrome";

            WebDriverFactory.InitDriver(browser);
            WebDriverFactory.Driver.Manage().Window.Maximize();

            wait = new WebDriverWait(WebDriverFactory.Driver, TimeSpan.FromSeconds(10));
            shortWait = new WebDriverWait(WebDriverFactory.Driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Stop()
        {
            if (WebDriverFactory.Driver != null)
                WebDriverFactory.Driver.Quit();
        }
    }
}