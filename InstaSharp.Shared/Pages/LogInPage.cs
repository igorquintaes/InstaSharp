using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class LogInPage : LoggedOutBasePage
    {
        private readonly HomePage homePage;
        private readonly SignUpPage signUpPage;

        protected override string UrlPath => "/accounts/login";

        public LogInPage(IWebDriver driver, HomePage homePage, SignUpPage signUpPage) : base(driver)
        {
            this.homePage = homePage;
            this.signUpPage = signUpPage;
        }

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

        public SignUpPage SignUpButton()
        {
            driver.FindElement(By.XPath("//form//button[./span[contains(@class, 'Facebook')]]")).Click();
            return signUpPage;
        }
    }
}
