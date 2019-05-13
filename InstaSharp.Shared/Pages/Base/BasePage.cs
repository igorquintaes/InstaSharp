using InstaSharp.Shared.Enum;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

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

        public void ChangeLanguage(object language, SelectType selectType)
        {
            var languageSelect = new SelectElement(
                driver.FindElement(By.CssSelector("footer select")));

            switch (selectType)
            {
                case SelectType.Index:
                    languageSelect.SelectByIndex(Convert.ToInt32(language));
                    break;
                case SelectType.Text:
                    languageSelect.SelectByText(language.ToString());
                    break;
                case SelectType.Value:
                    languageSelect.SelectByValue(language.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
