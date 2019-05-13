using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace InstaSharp.Shared.Extensions
{
    public static class IWebDriverExtensions
    {
        public static ITakesScreenshot Driver { get; private set; }

        public static WebDriverWait Wait(this IWebDriver driver) =>
            new WebDriverWait(driver, TimeSpan.FromSeconds(60));


        public static void Screenshot(this IWebDriver driver, string path) => 
            (Driver as ITakesScreenshot)
                .GetScreenshot()
                .SaveAsFile(path);
    }
}
