using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages.Base
{
    public abstract class BasePage
    {
        protected abstract string UrlPath { get; }
        protected readonly IWebDriver driver;
        private const string baseUrl = "https://www.instagram.com";

        public BasePage(IWebDriver driver) => 
            this.driver = driver;

        public void Navigate(params string[] args) => 
            driver.Navigate()
                  .GoToUrl(baseUrl + string.Format(UrlPath, args));
    }
}
