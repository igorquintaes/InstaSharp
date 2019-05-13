using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;

namespace InstaSharp.Shared.Pages
{
    public class SignUpPage : LoggedOutBasePage
    {
        private readonly FaceBookPage faceBookPage;
        private readonly HomePage homePage;
        private readonly LogInPage logInPage;

        public SignUpPage(IWebDriver driver, FaceBookPage faceBookPage, HomePage homePage, LogInPage logInPage) : base(driver)
        {
            this.faceBookPage = faceBookPage;
            this.homePage = homePage;
            this.logInPage = logInPage;
        }

        protected override string UrlPath => "/accounts/emailsignup/";

        public string MobileNumberOrEmail
        {
            get => driver.FindElement(By.Name("emailOrPhone")).GetAttribute("value");
            set => SetText(By.Name("emailOrPhone"), value);
        }

        public string FullName
        {
            get => driver.FindElement(By.Name("fullName")).GetAttribute("value");
            set => SetText(By.Name("fullName"), value);
        }
        public string UserName
        {
            get => driver.FindElement(By.Name("username")).GetAttribute("value");
            set => SetText(By.Name("username"), value);
        }
        public string Password
        {
            get => driver.FindElement(By.Name("password")).GetAttribute("value");
            set => SetText(By.Name("password"), value);
        }

        public HomePage SignUpButton()
        {
            driver.FindElement(By.XPath("//form//button[@type='submit']")).Click();
            return homePage;
        }

        public FaceBookPage LoginWithFaceBookButton()
        {
            driver.FindElement(By.XPath("//form//button[./span[contains(@class, 'Facebook')]]")).Click();
            return faceBookPage;
        }
    }
}
