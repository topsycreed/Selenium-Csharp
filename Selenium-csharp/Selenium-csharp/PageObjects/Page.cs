using OpenQA.Selenium.Support.PageObjects;
using Selenium_csharp.WrapperFactory;

namespace Selenium_csharp.PageObjects
{
    public static class Page
    {
        private static T GetPage<T>() where T : new()
        {
            var page = new T();
            PageFactory.InitElements(WebDriverFactory.Driver, page);
            return page;
        }

        public static MainPage Main
        {
            get { return GetPage<MainPage>(); }
        }

        public static ProductPage Product
        {
            get { return GetPage<ProductPage>(); }
        }

        public static BasketPage Basket
        {
            get { return GetPage<BasketPage>(); }
        }
    }
}
