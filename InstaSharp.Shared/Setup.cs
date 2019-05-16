using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InstaSharp.Shared
{
    public class Setup
    {
        public static IWebDriver Configure(bool isHeadless = false, string proxyIp = null)
        {
            var chromeLocation = GetBrowserDir("Selenium.WebDriver.ChromeDriver", "driver", "win32");
            var option = new ChromeOptions();
            var chromeService = ChromeDriverService.CreateDefaultService(chromeLocation);

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

        private static string GetBrowserDir(string packageName, params string[] dirs)
        {
            var nugetDir = GetNugetDir();
            var expandedDirs = Environment.ExpandEnvironmentVariables(nugetDir);
            var packagesDirs = Path.Combine(expandedDirs, "packages", packageName);
            var lastVersion = Directory.EnumerateDirectories(packagesDirs)
                                        .Select(p => new Version(Path.GetFileName(p)))
                                        .Max();
            var driversDir = Path.Combine(packagesDirs, lastVersion.ToString(), Path.Combine(dirs));
            return driversDir;
        }

        private static string GetNugetDir() =>
            Path.Combine(
                Environment.GetEnvironmentVariable("USERPROFILE")
                    ?? Environment.GetEnvironmentVariable("HOME"),
                       ".nuget"
            );
    }
}
