using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace InstaSharp.Shared
{
    public class Setup
    {
        public static IWebDriver Configure(bool isHeadless = false, string proxyIp = null)
        {
            var option = new ChromeOptions();
            var chromeService = ChromeDriverService.CreateDefaultService();

            option.AddArguments(
                "start-maximized",
                "--js-flags=--expose-gc",
                "--enable-precise-memory-info",
                "--disable-popup-blocking",
                "--disable-default-apps",
                "disable-infobars");

            if (isHeadless || !Debugger.IsAttached)
            {
                option.AddArguments(
                    "--headless",
                    "window-size=1366,768",
                    "log-level=3");

                chromeService.HideCommandPromptWindow = true;
            }

            if (!string.IsNullOrWhiteSpace(proxyIp))
                option.Proxy = new Proxy
                {
                    HttpProxy = proxyIp,
                    SslProxy = proxyIp
                };

            return new ChromeDriver(chromeService, option);
        }
    }
}
