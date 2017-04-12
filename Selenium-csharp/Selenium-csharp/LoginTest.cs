using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Selenium_csharp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Selenium_csharp.Helpers;
using System.IO;
using System.Threading;

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

            //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory, "geckodriver.exe");
            //driver = new FirefoxDriver(service);

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
                int zonesCount = int.Parse(zones.GetAttribute("innerText"));

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
                int zonesCount = int.Parse(zones.GetAttribute("innerText"));

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

            //First product in Campaigns
            IWebElement product = driver.FindElement(By.XPath("//div[@id = 'box-campaigns']//a[@class = 'link']/div[@class='name']/.."));

            //Get elements and attributes from Main page
            string nameMainPage = product.FindElement(By.XPath(".//div[@class = 'name']")).GetAttribute("innerText");
            IWebElement campaignPriceMainPage = product.FindElement(By.XPath(".//strong[@class = 'campaign-price']"));
            IWebElement regularPriceMainPage = product.FindElement(By.XPath(".//s[@class = 'regular-price']"));
            string valueOfCampaignPriceMainPage = campaignPriceMainPage.GetAttribute("innerText");
            string valueOfRegularPriceMainPage = regularPriceMainPage.GetAttribute("innerText");
            string colorCampaignPriceMainPage = campaignPriceMainPage.GetCssValue("color");
            string colorRegularPriceMainPage = regularPriceMainPage.GetCssValue("color");
            string fontSizeCampaignPriceMainPage = campaignPriceMainPage.GetCssValue("font-size");
            string fontSizeRegularPriceMainPage = regularPriceMainPage.GetCssValue("font-size");

            //Asserts on Main page
            Assert.That(ColorHelpers.isGray(colorRegularPriceMainPage), Is.True);
            Assert.That(ColorHelpers.isRed(colorCampaignPriceMainPage), Is.True);
            Assert.That(FontHelpers.GetFontSizeValue(fontSizeCampaignPriceMainPage), Is.GreaterThan(FontHelpers.GetFontSizeValue(fontSizeRegularPriceMainPage)));

            product.Click();

            //Product box on Product page
            IWebElement productBox = driver.FindElement(By.XPath("//div[@id = 'box-product']"));

            //Get elements and attributes from Product page
            string nameProductPage = productBox.FindElement(By.XPath(".//h1")).GetAttribute("innerText");
            IWebElement campaignPriceProductPage = productBox.FindElement(By.XPath(".//strong[@class = 'campaign-price']"));
            IWebElement regularPriceProductPage = productBox.FindElement(By.XPath(".//s[@class = 'regular-price']"));
            string valueOfCampaignPriceProductPage = campaignPriceProductPage.GetAttribute("innerText");
            string valueOfRegularPriceProductPage = regularPriceProductPage.GetAttribute("innerText");
            string colorCampaignPriceProductPage = campaignPriceProductPage.GetCssValue("color");
            string colorRegularPriceProductPage = regularPriceProductPage.GetCssValue("color");
            string fontSizeCampaignPriceProductPage = campaignPriceProductPage.GetCssValue("font-size");
            string fontSizeRegularPriceProductPage = regularPriceProductPage.GetCssValue("font-size");

            //Asserts on Product page
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

            //Generation of unique email
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

            //Get element from select without using SelectElement
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

            //Login with data from registration
            IWebElement emailLogin = driver.FindElement(By.XPath("//input[@name = 'email']"));
            IWebElement passwordLogin = driver.FindElement(By.XPath("//input[@name = 'password']"));
            IWebElement login = driver.FindElement(By.XPath("//button[@name = 'login']"));

            emailLogin.SendKeys(generatedEmail);
            passwordLogin.SendKeys(_password);
            login.Click();

            //Refresh logout to avoid StaleElementsException
            logout = driver.FindElement(By.XPath("//a[contains(@href,'logout')]"));
            logout.Click();
        }

        [Test]
        public void AddProduct()
        {
            Login();

            IWebElement catalog = driver.FindElement(By.XPath("//span[@class = 'name' and text() = 'Catalog']"));

            catalog.Click();

            IWebElement addNewProduct = driver.FindElement(By.XPath("//a[@class = 'button' and contains(@href, 'edit_product')]"));

            addNewProduct.Click();

            IWebElement nameOfProduct = driver.FindElement(By.XPath("//input[@name = 'name[en]']"));
            IWebElement codeOfProduct = driver.FindElement(By.XPath("//input[@name = 'code']"));
            IWebElement femaleChecbox = driver.FindElement(By.XPath("//input[@name = 'product_groups[]' and @type = 'checkbox' and @value = '1-2']"));
            IWebElement maleChecbox = driver.FindElement(By.XPath("//input[@name = 'product_groups[]' and @type = 'checkbox' and @value = '1-1']"));
            IWebElement unisexChecbox = driver.FindElement(By.XPath("//input[@name = 'product_groups[]' and @type = 'checkbox' and @value = '1-3']"));
            IWebElement quantity = driver.FindElement(By.XPath("//input[@name = 'quantity']"));
            //IWebElement quantityUnit = driver.FindElement(By.XPath("//select[@name = 'quantity_unit_id']"));
            //IWebElement deliveryStatus = driver.FindElement(By.XPath("//select[@name = 'delivery_status_id']"));
            //IWebElement soldOutStatus = driver.FindElement(By.XPath("//select[@name = 'sold_out_status_id']"));
            IWebElement chouseFile = driver.FindElement(By.XPath("//input[@name = 'new_images[]']"));
            IWebElement dataValidFrom = driver.FindElement(By.XPath("//input[@name = 'date_valid_from']"));
            IWebElement dataValidTo = driver.FindElement(By.XPath("//input[@name = 'date_valid_to']"));

            nameOfProduct.SendKeys("Test");
            codeOfProduct.SendKeys("123");
            femaleChecbox.Click();
            maleChecbox.Click();
            unisexChecbox.Click();
            quantity.SendKeys("10");

            //Upload image
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = string.Format("{0}Resources\\rubic.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            chouseFile.SendKeys(FileName);

            dataValidFrom.SendKeys("02.04.2017");
            dataValidTo.SendKeys("03.04.2017");

            IWebElement information = driver.FindElement(By.XPath("//a[@href = '#tab-information']"));

            information.Click();
            
            IWebElement manufacter = driver.FindElement(By.XPath("//select[@name = 'manufacturer_id']"));
            IWebElement keywords = driver.FindElement(By.XPath("//input[@name = 'keywords']"));
            IWebElement shortDescription = driver.FindElement(By.XPath("//input[@name = 'short_description[en]']"));
            IWebElement description = driver.FindElement(By.XPath("//div[@class = 'trumbowyg-editor']"));
            IWebElement headTitle = driver.FindElement(By.XPath("//input[@name = 'head_title[en]']"));
            IWebElement metaDescription = driver.FindElement(By.XPath("//input[@name = 'meta_description[en]']"));

            //Select element using SelectElement
            SelectElement selectManufacter = new SelectElement(manufacter);
            selectManufacter.SelectByText("ACME Corp.");

            keywords.SendKeys("rubic");
            shortDescription.SendKeys("Test short description");
            description.SendKeys("Test full description");
            headTitle.SendKeys("Title");
            metaDescription.SendKeys("meta desc");

            IWebElement prices = driver.FindElement(By.XPath("//a[@href = '#tab-prices']"));

            prices.Click();

            IWebElement price = driver.FindElement(By.XPath("//input[@name = 'purchase_price']"));
            IWebElement priceType = driver.FindElement(By.XPath("//select[@name = 'purchase_price_currency_code']"));


            price.SendKeys("10");

            //Select element using SelectElement
            SelectElement selectpriceType = new SelectElement(priceType);
            selectpriceType.SelectByText("US Dollars");

            IWebElement save = driver.FindElement(By.XPath("//button[@name = 'save']"));

            save.Click();

            IWebElement addedProduct = driver.FindElement(By.XPath("//a[text() = 'Test']"));

            addedProduct.Click();

            IWebElement delete = driver.FindElement(By.XPath("//button[@name = 'delete']"));

            delete.Click();

            //Accept alert
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
        }

        [Test]
        public void DeleteItemFromRecycleBin()
        {
            driver.Url = "http://localhost/litecart/en/";

            for (int i = 0; i < 3; i++)
            {
                //First product (may be different becouse list change order every time)
                IWebElement product = driver.FindElement(By.XPath("//a[@class = 'link']/div[@class='name']/.."));

                product.Click();

                //If page contains required field Size - fill it
                if (AreElementsPresent(driver, By.XPath("//select[@name = 'options[Size]']")))
                {
                    IWebElement size = driver.FindElement(By.XPath("//select[@name = 'options[Size]']"));
                    SelectElement selectSize = new SelectElement(size);
                    selectSize.SelectByText("Small");
                }

                IWebElement addToCart = driver.FindElement(By.XPath("//button[@name = 'add_cart_product']"));

                //Quantity of adding product (by default = 1)
                IWebElement productQuantityElement = driver.FindElement(By.XPath("//div[@class = 'content']//input[@name = 'quantity']"));
                int productQuantity = int.Parse(productQuantityElement.GetAttribute("valueAsNumber"));

                //Quantity of already added product in Cart
                IWebElement cart = driver.FindElement(By.XPath("//span[@class = 'quantity']"));
                int cartQuantity = int.Parse(cart.GetAttribute("innerText"));

                addToCart.Click();
                //Wait to check, that quantity in Cart increased by Quantity value from product
                wait.Until(e => int.Parse(e.FindElement(By.XPath("//span[@class = 'quantity']")).GetAttribute("innerText")).Equals(cartQuantity + productQuantity));
            }

            IWebElement checkout = driver.FindElement(By.XPath("//a[@class = 'link' and contains(@href, 'checkout')]"));

            checkout.Click();

            //Read forms with product
            ReadOnlyCollection<IWebElement> itemsForm = driver.FindElements(By.XPath("//form[@name = 'cart_form']"));

            if (itemsForm.Count() > 1)
            {
                for (int i = 0; i < itemsForm.Count(); i++)
                {
                    IWebElement itemRemoveButton = itemsForm[i].FindElement(By.XPath(".//button[@name = 'remove_cart_item']"));

                    IWebElement itemNameElement = itemsForm[i].FindElement(By.XPath(".//strong"));
                    string itemName = itemNameElement.GetAttribute("innerText");

                    //Read shortcats of products
                    IWebElement shortcatForm = driver.FindElement(By.XPath("//ul[@class = 'shortcuts']"));
                    ReadOnlyCollection<IWebElement> shortcats = shortcatForm.FindElements(By.XPath(".//a"));

                    int j = 0;

                    //Run through the list of shortcats to fing product with clickable remove
                    while (!itemRemoveButton.Displayed && j < shortcats.Count())
                    {
                        try
                        {
                            shortcats[j].Click();
                            WebDriverWait waitForRemove = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                            waitForRemove.Until(ExpectedConditions.ElementToBeClickable(itemRemoveButton));
                        }
                        catch (WebDriverTimeoutException)
                        {
                            j++;
                        }
                    }
                    itemRemoveButton.Click();

                    //Wait to check that product also deleted from table
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//td[@class = 'item' and text() = '" + itemName + "']")));

                    //Refresh product forms to avoid StaleElementException
                    itemsForm = driver.FindElements(By.XPath("//form[@name = 'cart_form']"));
                }
            }

            //If finded/left only 1 form - there is no shortcats, just click delete button
            if (itemsForm.Count() == 1)
            {
                IWebElement itemRemoveButton = itemsForm[0].FindElement(By.XPath(".//button[@name = 'remove_cart_item']"));

                IWebElement itemNameElement = itemsForm[0].FindElement(By.XPath(".//strong"));
                string itemName = itemNameElement.GetAttribute("innerText");

                itemRemoveButton.Click();

                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//td[@class = 'item' and text() = '" + itemName + "']")));
            }

            Assert.That(driver.FindElement(By.XPath("//em")).Text, Is.EqualTo("There are no items in your cart."));
        }

        [Test]
        public void CheckOpenInNewWindows()
        {
            Login();

            IWebElement CountriesPage = driver.FindElement(By.XPath("//span[text() = 'Countries']"));
            CountriesPage.Click();

            IWebElement AddNewCountryButton = driver.FindElement(By.XPath("//a[@class = 'button' and contains(@href, 'edit_country')]"));
            AddNewCountryButton.Click();

            ReadOnlyCollection<IWebElement> externalLinks = driver.FindElements(By.XPath("//i[@class = 'fa fa-external-link']"));

            //Get Id of current active window
            string mainWindow = driver.CurrentWindowHandle;

            for (int i = 0; i < externalLinks.Count(); i++)
            {
                externalLinks[i].Click();
                //wait until new window will opened
                wait.Until(driver => driver.WindowHandles.Count() == 2);

                ReadOnlyCollection<string> Windows = driver.WindowHandles;
                //find index of new window in windows collection and switch to it
                for (int j = 0; j < Windows.Count(); j++)
                {
                    if (Windows[j] != mainWindow)
                    {
                        driver.SwitchTo().Window(Windows[j].ToString());
                    }
                }
                //sleep to see that new windows really opens
                Thread.Sleep(TimeSpan.FromSeconds(1));

                //close new window and set main window active
                driver.Close();
                driver.SwitchTo().Window(mainWindow);

                //Refresh externalLinks to avoid StaleElementsException
                externalLinks = driver.FindElements(By.XPath("//i[@class = 'fa fa-external-link']"));
            }
        }

        [Test]
        public void CheckBrowserLogs()
        {
            Login();

            IWebElement catalog = driver.FindElement(By.XPath("//span[@class = 'name' and text() = 'Catalog']"));

            catalog.Click();

            ReadOnlyCollection<IWebElement> products = driver.FindElements(By.XPath("//a[contains(@href, 'edit_product') and @title = 'Edit']"));

            int countOfBrowserLogs = 0;

            for (int i = 0; i < products.Count(); i++)
            {
                products[i].Click();

                foreach (LogEntry l in driver.Manage().Logs.GetLog("browser"))
                {
                    Console.WriteLine(l);
                    countOfBrowserLogs++;
                }

                //Refresh products to avoid StaleElementsException
                products = driver.FindElements(By.XPath("//a[contains(@href, 'edit_product') and @title = 'Edit']"));
            }

            Assert.That(countOfBrowserLogs, Is.EqualTo(0));
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
