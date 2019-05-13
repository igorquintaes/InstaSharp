using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class LogInPage : LoggedOutBasePage
    {
        protected override string UrlPath => "/";

        public LogInPage(IWebDriver driver) : base(driver) { }

        public string username
        {
            get => driver.FindElement(By.Name("username")).Text;
            set => SetText(By.Name("username"), value);
        }

        public string password
        {
            get => driver.FindElement(By.Name("password")).Text;
            set => SetText(By.Name("password"), value);
        }

        public void LoginButton() => driver.FindElement(By.XPath("//form//button[@type='submit']")).Click();

        public void LoginWithFaceBookButton() => driver.FindElement(By.XPath("//form//span[contains(@class, 'coreSpriteFacebookIcon')]")).Click();
    }
}
