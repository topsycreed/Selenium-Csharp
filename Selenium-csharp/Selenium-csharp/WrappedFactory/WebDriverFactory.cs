using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;

namespace Selenium_csharp.WrapperFactory
{
    public class WebDriverFactory
    {
        private static IWebDriver driver;

        public static IWebDriver Driver
        {
            get
            {
                if (driver == null)
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method InitDriver.");
                return driver;
            }
            private set
            {
                driver = value;
            }
        }

        public static void InitDriver(string driverName)
        {
            switch (driverName)
            {
                case "Firefox":
                    if (driver == null)
                    {
                        FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory, "geckodriver.exe");
                        driver = new FirefoxDriver(service);
                    }
                    break;

                case "IE":
                    if (driver == null)
                    {
                        driver = new InternetExplorerDriver(AppDomain.CurrentDomain.BaseDirectory);
                    }
                    break;

                case "Chrome":
                    if (driver == null)
                    {
                        driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);
                    }
                    break;
            }
        }

        public static void CloseDriver()
        {
            if (Driver != null)
                Driver.Quit();
            Driver = null;
        }
    }
}
