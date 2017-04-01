using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Selenium_csharp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Selenium_csharp.Helpers;

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
                            //Refresh elements to avoid StaleElementsException
                            subPagesToClick = driver.FindElements(By.XPath("//ul[@id = 'box-apps-menu']/li/ul//span[@class = 'name']"));
                        }
                    }
                    //Refresh elements to avoid StaleElementsException
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

        [Test]
        public void CheckCountries()
        {
            Login();

            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            ReadOnlyCollection<IWebElement> countryRows = driver.FindElements(By.XPath("//tr[@class = 'row']"));
            int countryRowsCount = countryRows.Count();
            List<string> countriesName = new List<string>();

            for (int i = 0; i < countryRowsCount; i++)
            {
                var country = countryRows[i].FindElement(By.XPath(".//a[string-length(text()) > 0]"));
                var zones = countryRows[i].FindElement(By.XPath(".//td[6]"));
                int zonesCount = Int32.Parse(zones.GetAttribute("innerText"));

                countriesName.Add(country.GetAttribute("innerText"));

                if (zonesCount > 0)
                {
                    country.Click();

                    ReadOnlyCollection<IWebElement> zoneRows = driver.FindElements(By.XPath("//table[@id = 'table-zones']//tr//input[contains(@name, 'zones')]/../.."));
                    int zoneRowsCount = zoneRows.Count();
                    List<string> zonesName = new List<string>();

                    for (int j = 0; j < zoneRowsCount; j++)
                    {
                        var zone = zoneRows[j].FindElement(By.XPath(".//input[contains(@name, '[name]')]"));

                        zonesName.Add(zone.GetAttribute("defaultValue"));
                    }
                    Assert.That(zonesName, Is.Ordered);
                    driver.Navigate().Back();
                    //Refresh elements to avoid StaleElementsException
                    countryRows = driver.FindElements(By.XPath("//tr[@class = 'row']"));
                }
            }

            Assert.That(countriesName, Is.Ordered);
        }

        [Test]
        public void CheckGeoZones()
        {
            Login();

            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            ReadOnlyCollection<IWebElement> countryRows = driver.FindElements(By.XPath("//tr[@class = 'row']"));
            int countryRowsCount = countryRows.Count();

            for (int i = 0; i < countryRowsCount; i++)
            {
                var country = countryRows[i].FindElement(By.XPath(".//a[string-length(text()) > 0]"));
                var zones = countryRows[i].FindElement(By.XPath(".//td[4]"));
                int zonesCount = Int32.Parse(zones.GetAttribute("innerText"));

                if (zonesCount > 0)
                {
                    country.Click();

                    ReadOnlyCollection<IWebElement> zoneRows = driver.FindElements(By.XPath("//table[@id = 'table-zones']//tr//input[contains(@name, 'zones')]/../.."));
                    int zoneRowsCount = zoneRows.Count();
                    List<string> zonesName = new List<string>();

                    for (int j = 0; j < zoneRowsCount; j++)
                    {
                        var zone = zoneRows[j].FindElement(By.XPath(".//select[contains(@name, 'zone_code')]/option[@selected = 'selected']"));

                        zonesName.Add(zone.GetAttribute("innerText"));
                    }
                    Assert.That(zonesName, Is.Ordered);
                    driver.Navigate().Back();
                    //Refresh elements to avoid StaleElementsException
                    countryRows = driver.FindElements(By.XPath("//tr[@class = 'row']"));
                }
            }         
        }

        [Test]
        public void CheckProduct()
        {
            driver.Url = "http://localhost/litecart/en/";

            //Первый товар в разделе Campaigns
            IWebElement product = driver.FindElement(By.XPath("//div[@id = 'box-campaigns']//a[@class = 'link']/div[@class='name']/.."));

            //Текст названия товара на главной
            string nameMainPage = product.FindElement(By.XPath(".//div[@class = 'name']")).GetAttribute("innerText");
            //Акционная цена на главной
            IWebElement campaignPriceMainPage = product.FindElement(By.XPath(".//strong[@class = 'campaign-price']"));
            //Обычная цена на главной
            IWebElement regularPriceMainPage = product.FindElement(By.XPath(".//s[@class = 'regular-price']"));
            //Размер акционной цены на главной
            string valueOfCampaignPriceMainPage = campaignPriceMainPage.GetAttribute("innerText");
            //Размер обычной цены на главной
            string valueOfRegularPriceMainPage = regularPriceMainPage.GetAttribute("innerText");
            //Цвет акционной цены на главной
            string colorCampaignPriceMainPage = campaignPriceMainPage.GetCssValue("color");
            //Цвет обычной цены на главной
            string colorRegularPriceMainPage = regularPriceMainPage.GetCssValue("color");
            //Размер акционной цены на главной
            string fontSizeCampaignPriceMainPage = campaignPriceMainPage.GetCssValue("font-size");
            //Размер обычной цены на главной
            string fontSizeRegularPriceMainPage = regularPriceMainPage.GetCssValue("font-size");

            //Asserts on main page
            Assert.That(ColorHelpers.isGray(colorRegularPriceMainPage), Is.True);
            Assert.That(ColorHelpers.isRed(colorCampaignPriceMainPage), Is.True);
            Assert.That(FontHelpers.GetFontSizeValue(fontSizeCampaignPriceMainPage), Is.GreaterThan(FontHelpers.GetFontSizeValue(fontSizeRegularPriceMainPage)));

            product.Click();

            //Блок с описанием товара на странице товара
            IWebElement productBox = driver.FindElement(By.XPath("//div[@id = 'box-product']"));

            //Текст названия товара на странице товара
            string nameProductPage = productBox.FindElement(By.XPath(".//h1")).GetAttribute("innerText");
            //Акционная цена на странице товара
            IWebElement campaignPriceProductPage = productBox.FindElement(By.XPath(".//strong[@class = 'campaign-price']"));
            //Обычная цена на странице товара
            IWebElement regularPriceProductPage = productBox.FindElement(By.XPath(".//s[@class = 'regular-price']"));
            //Размер акционной цены на странице товара
            string valueOfCampaignPriceProductPage = campaignPriceProductPage.GetAttribute("innerText");
            //Размер обычной цены на странице товара
            string valueOfRegularPriceProductPage = regularPriceProductPage.GetAttribute("innerText");
            //Цвет акционной цены на странице товара
            string colorCampaignPriceProductPage = campaignPriceProductPage.GetCssValue("color");
            //Цвет обычной цены на странице товара
            string colorRegularPriceProductPage = regularPriceProductPage.GetCssValue("color");
            //Размер акционной цены на странице товара
            string fontSizeCampaignPriceProductPage = campaignPriceProductPage.GetCssValue("font-size");
            //Размер обычной цены на странице товара
            string fontSizeRegularPriceProductPage = regularPriceProductPage.GetCssValue("font-size");

            //Asserts on product page
            Assert.That(nameMainPage, Is.EqualTo(nameProductPage));
            Assert.That(valueOfCampaignPriceMainPage, Is.EqualTo(valueOfCampaignPriceProductPage));
            Assert.That(valueOfRegularPriceMainPage, Is.EqualTo(valueOfRegularPriceProductPage));
            Assert.That(ColorHelpers.isGray(colorRegularPriceProductPage), Is.True);
            Assert.That(ColorHelpers.isRed(colorCampaignPriceProductPage), Is.True);
            Assert.That(FontHelpers.GetFontSizeValue(fontSizeCampaignPriceProductPage), Is.GreaterThan(FontHelpers.GetFontSizeValue(fontSizeRegularPriceProductPage)));
        }

        [Test]
        public void RegisterUser()
        {
            driver.Url = "http://localhost/litecart/en/create_account";
            //Генерация уникального email
            string generatedEmail = "gena" + DateTime.Now.ToString("hmmsstt") + "@mail.ru";
            string _password = "password";

            IWebElement firstName = driver.FindElement(By.XPath("//input[@name = 'firstname']"));
            IWebElement lastName = driver.FindElement(By.XPath("//input[@name = 'lastname']"));
            IWebElement address1 = driver.FindElement(By.XPath("//input[@name = 'address1']"));
            IWebElement postcode = driver.FindElement(By.XPath("//input[@name = 'postcode']"));
            IWebElement city = driver.FindElement(By.XPath("//input[@name = 'city']"));
            IWebElement country = driver.FindElement(By.XPath("//span[@role = 'combobox']"));
            
            IWebElement email = driver.FindElement(By.XPath("//input[@name = 'email']"));
            IWebElement phone = driver.FindElement(By.XPath("//input[@name = 'phone']"));
            IWebElement password = driver.FindElement(By.XPath("//input[@name = 'password']"));
            IWebElement confirmedPassword = driver.FindElement(By.XPath("//input[@name = 'confirmed_password']"));
            IWebElement createAccount = driver.FindElement(By.XPath("//button[@name = 'create_account']"));

            firstName.SendKeys("Gena");
            lastName.SendKeys("Topsycreed");
            address1.SendKeys("Saratov");
            postcode.SendKeys("41009");
            city.SendKeys("Saratov");
            //Выбор элемента из списка
            country.Click();
            driver.FindElement(By.XPath("//input[@type = 'search']")).SendKeys("United States");
            driver.FindElement(By.XPath(".//li[text() = 'United States']")).Click();

            email.SendKeys(generatedEmail);
            phone.SendKeys("+79196215502");
            password.SendKeys(_password);
            confirmedPassword.SendKeys("password");

            createAccount.Click();

            IWebElement logout = driver.FindElement(By.XPath("//a[contains(@href,'logout')]"));

            logout.Click();
            //Логин с данными из регистрации
            IWebElement emailLogin = driver.FindElement(By.XPath("//input[@name = 'email']"));
            IWebElement passwordLogin = driver.FindElement(By.XPath("//input[@name = 'password']"));
            IWebElement login = driver.FindElement(By.XPath("//button[@name = 'login']"));

            emailLogin.SendKeys(generatedEmail);
            passwordLogin.SendKeys(_password);
            login.Click();

            //Обновление ссылки на logout
            logout = driver.FindElement(By.XPath("//a[contains(@href,'logout')]"));
            logout.Click();
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
