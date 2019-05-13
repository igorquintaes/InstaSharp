using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages.Base
{
    public abstract class LoggedInBasePage : BasePage
    {
        public LoggedInBasePage(IWebDriver driver)
            : base(driver)
        { }
    }
}
