using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class LogInPage : LoggedOutBasePage
    {
        protected override string UrlPath => "/";

        public LogInPage(IWebDriver driver)
            : base(driver)
        { }
    }
}
