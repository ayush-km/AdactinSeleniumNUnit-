using static AdactinSeleniumNUnit.BasePage;

namespace AdactinSeleniumNUnit;

[SetUpFixture] // Assembly-level setup/cleanup
public class AssemblySetup
{
    [OneTimeSetUp]
    public void AssemblyInitialize()
    {
        var resultFilePath = @"C:\ExtentReports\" + Globals.AppName + @"\TestExecLog_" +
                             DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
        CreateReport(resultFilePath);
    }

    [OneTimeTearDown]
    public void AssemblyCleanup()
    {
        ExtentReports.Flush();
    }
}