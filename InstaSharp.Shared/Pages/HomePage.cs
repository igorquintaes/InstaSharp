using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class HomePage : LoggedInBasePage
    {
        protected override string UrlPath => "/";

        public HomePage(IWebDriver driver)
            : base(driver)
        { }
    }
}
