using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class LogInPage : LoggedOutBasePage
    {
        private readonly HomePage homePage;
        private readonly SignUpPage signUpPage;
        private readonly FaceBookPage faceBookPage;

        protected override string UrlPath => "/accounts/login";

        public LogInPage(IWebDriver driver, HomePage homePage, SignUpPage signUpPage, FaceBookPage faceBookPage) : base(driver)
        {
            this.homePage = homePage;
            this.signUpPage = signUpPage;
            this.faceBookPage = faceBookPage;
        }

        public string Username
        {
            get => driver.FindElement(By.Name("username")).Text;
            set => SetText(By.Name("username"), value);
        }

        public string Password
        {
            get => driver.FindElement(By.Name("password")).GetAttribute("value");
            set => SetText(By.Name("password"), value);
        }

        public HomePage LoginButton()
        {
            driver.FindElement(By.XPath("//form//button[@type='submit']")).Click();
            return homePage;
        }

        public FaceBookPage LoginWithFaceBookButton()
        {
            driver.FindElement(By.XPath("//form//button[./span[contains(@class, 'Facebook')]]")).Click();
            return faceBookPage;
        }

        public SignUpPage SignUpButton()
        {
            driver.FindElement(By.XPath("//form//button[./span[contains(@class, 'Facebook')]]")).Click();
            return signUpPage;
        }
    }
}
