using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace AdactinSeleniumNUnit;

public class BasePage
{
    public static IWebDriver Driver;
    public static ExtentReports ExtentReports;
    public static ExtentTest Test;
    public static ExtentTest Step;

    public static IWebDriver InitializeSelenium(string browser, bool headless = true)
    {
        switch (browser)
        {
            case "Firefox":
            {
                var firefoxOptions = new FirefoxOptions();
                firefoxOptions.AddArguments("-private-window");
                if (headless) firefoxOptions.AddArguments("-headless");
                var firefoxDriver = new FirefoxDriver(firefoxOptions);
                firefoxDriver.Manage().Window.Maximize();
                return firefoxDriver;
            }
            case "MSEdge":
            {
                var edgeOptions = new EdgeOptions();
                edgeOptions.AddArguments("--start-maximized");
                edgeOptions.AddArguments("--inprivate");
                if (headless) edgeOptions.AddArguments("--headless");
                var edgeDriver = new EdgeDriver(edgeOptions);
                return edgeDriver;
            }
            default:
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("--start-maximized");
                chromeOptions.AddArguments("--incognito");
                if (headless) chromeOptions.AddArguments("--headless");
                var chromeDriver = new ChromeDriver(chromeOptions);
                return chromeDriver;
        }
    }

    public static void OpenUrl(string url)
    {
        Driver.Url = url;
        TakeScreenshot();
    }

    public static void Write(By by, string value, string stepDetail = "N/A")
    {
        try
        {
            Driver.FindElement(by).SendKeys(value);
            TakeScreenshot(Status.Pass, stepDetail);
        }
        catch (Exception ex)
        {
            TakeScreenshot(Status.Fail, "FAILED: " + stepDetail + ex);
        }
    }

    public static void Click(By by, string stepDetail = "N/A")
    {
        try
        {
            Driver.FindElement(by).Click();
            TakeScreenshot(Status.Pass, stepDetail);
        }
        catch (Exception ex)
        {
            TakeScreenshot(Status.Fail, "FAILED: " + stepDetail + ex);
        }
    }

    public static string GetText(string locatorType, string locatorValue, string textSource)
    {
        var locator = GetLocator(locatorType, locatorValue);

        return textSource.ToLower() switch
        {
            "text" => Driver.FindElement(locator).Text,
            "value" => Driver.FindElement(locator).GetAttribute("Value"),
            _ => throw new ArgumentException($"Unsupported text source: {locatorType}")
        };
    }


    public static void CreateReport(string dirpath)
    {
        ExtentReports = new ExtentReports();

        var sparkReporter = new ExtentSparkReporter(dirpath);
        ExtentReports.AttachReporter(sparkReporter);
    }

    private static void TakeScreenshot()
    {
        var directoryPath = @"C:\ExtentReports\" + Globals.AppName + @"\images\";
        var filePath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
        File.WriteAllBytes(filePath, screenshot.AsByteArray);
    }

    private static void TakeScreenshot(Status status, string stepDetail)
    {
        const string directoryPath = @"C:\ExtentReports\AutomationSelenium\images\";
        var filePath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
        File.WriteAllBytes(filePath, screenshot.AsByteArray);
        Step.Log(status, stepDetail, MediaEntityBuilder.CreateScreenCaptureFromPath(filePath).Build());
    }

    /*public static void TakeScreenshot(Status status, string stepDetail)
    {
        string path = @"C:\ExtentReports\AutomationSelenium\images\" +
            DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
        File.WriteAllBytes(path, screenshot.AsByteArray);
        Step.Log(status, stepDetail, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
    }*/

    public static void TerminateSelenium()
    {
        Driver.Close();
        Driver.Dispose();
        Driver.Quit();
    }

    private static By GetLocator(string locatorType, string locatorValue)
    {
        return locatorType.ToLower() switch
        {
            "id" => By.Id(locatorValue),
            "name" => By.Name(locatorValue),
            "classname" or "class" => By.ClassName(locatorValue),
            "xpath" => By.XPath(locatorValue),
            "cssselector" or "css" => By.CssSelector(locatorValue),
            "tagname" or "tag" => By.TagName(locatorValue),
            "linktext" => By.LinkText(locatorValue),
            "partiallinktext" => By.PartialLinkText(locatorValue),
            _ => throw new ArgumentException($"Unsupported locator type: {locatorType}")
        };
    }
}