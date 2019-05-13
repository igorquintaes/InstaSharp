using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages.Base
{
    public abstract class LoggedOutBasePage : BasePage
    {
        public LoggedOutBasePage(IWebDriver driver)
            : base(driver)
        { }
    }
}
