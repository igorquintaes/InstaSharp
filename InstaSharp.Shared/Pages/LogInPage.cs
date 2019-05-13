using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class LogInPage : LoggedOutBasePage
    {
        private readonly HomePage homePage;

        protected override string UrlPath => "/";

        public LogInPage(IWebDriver driver, HomePage homePage) : base(driver) => this.homePage = homePage;

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

        public HomePage LoginButton()
        {
            driver.FindElement(By.XPath("//form//button[@type='submit']")).Click();
            return homePage;
        }

        public void LoginWithFaceBookButton() => driver.FindElement(By.XPath("//form//button[./span[contains(@class, 'Facebook')]]")).Click();
    }
}
