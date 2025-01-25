using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AdactinSeleniumNUnit;

public static class Globals
{
    public const string AppName = "AdactinSeleniumNUnit";

    public static bool IsBrowserInstalled(string browser)
    {
        return browser.ToLower() switch
        {
            "chrome" => File.Exists(@"C:\Program Files\Google\Chrome\Application\chrome.exe"),
            "firefox" => File.Exists(@"C:\Program Files\Mozilla Firefox\firefox.exe"),
            "msedge" => File.Exists(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"),
            _ => throw new ArgumentException($"Unsupported browser: {browser.ToLower()}")
        };
    }

    public static void WaitForElement(By locator, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(BasePage.Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(d => d.FindElement(locator).Displayed);
    }
}