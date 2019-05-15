using InstaSharp.Shared.Pages.Base;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

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
            SkipInstagranDownload(driver);
            SkipInstagranNotifications(driver);
            return homePage;

            void SkipInstagranDownload(IWebDriver driver)
            {
                if (driver.Url == "https://www.instagram.com/#reactivated")
                    driver.FindElement(By.XPath("//a[@href='/']")).Click();
            }

            void SkipInstagranNotifications(IWebDriver driver)
            {
                var notificationPopUp = driver
                    .FindElements(By.XPath("//div[@role='presentation']//button[2]"))
                    .FirstOrDefault(x => x.Displayed);

                if (notificationPopUp != null)
                {
                    notificationPopUp.Click();
                    Thread.Sleep(3000);
                }
            }
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
