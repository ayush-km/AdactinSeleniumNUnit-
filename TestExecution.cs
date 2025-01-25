using System.Configuration;
using System.Text.Json;
using AdactinSeleniumNUnit.AdactinHotel.BookingPage;
using AdactinSeleniumNUnit.AdactinHotel.LoginPage;
using AdactinSeleniumNUnit.AdactinHotel.SearchPage;
using AdactinSeleniumNUnit.AdactinHotel.SelectPage;
using AventStack.ExtentReports;
using static AdactinSeleniumNUnit.BasePage;

namespace AdactinSeleniumNUnit;

public class TestExecution
{
    #region Setups and Cleanups Test-Level
    
    [SetUp]
    public void Setup()
    {
        var configFileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Globals.AppName + ".dll.config")
        };
        var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        var appSettings = config.AppSettings.Settings;
        if (appSettings.Count == 0)
        {
            Console.WriteLine("AppSettings is empty. Check the configuration file.");
        }
        else
        {
            var browser = appSettings["Browser"]?.Value ?? "Chrome";
            Assert.That(browser, Is.Not.Null, "Browser setting not found in App.config.");
            Console.WriteLine($"Browser: {browser}");
            if (Globals.IsBrowserInstalled(browser))
            {
                Driver = InitializeSelenium(browser);
                Test = BasePage.ExtentReports.CreateTest(TestContext.CurrentContext.Test.Name);
            }
            else
            {
                Console.WriteLine($"{browser} Browser is not installed. Supported: Chrome, Firefox, MSEdge");
            }
        }
        /*var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        Console.WriteLine($"Configuration file path: {configFile.FilePath}");
        //Console.WriteLine($"Browser: {ConfigurationManager.AppSettings.Count}");
        var browser = ConfigurationManager.AppSettings["Browser"] ?? "Chrome";
        InitializeSelenium(browser);
        Test = ExtentReports.CreateTest(TestContext.CurrentContext.Test.Name);*/
    }

    [TearDown]
    public void TearDown()
    {
        TerminateSelenium();
    }

    #endregion

    private readonly LoginPage _loginPage = new();
    private readonly SearchPage _searchPage = new();
    private readonly SelectPage _selectPage = new();
    private readonly BookingPage _bookingPage = new();

    [Test, TestCaseSource(nameof(GetLoginTestData))]
    public void TC01_Login(LoginTestData loginTestData)
    {
        _loginPage.Login(loginTestData.Url, loginTestData.Username, loginTestData.Password);
        var actualMessage = GetText(loginTestData.Result.LocatorType, loginTestData.Result.LocatorValue,
            loginTestData.Result.TextSource);
        Assert.That(actualMessage, Is.EqualTo(loginTestData.Result.ExpectedMessage));
        Test.Log(Status.Info, $"Test Data: {JsonSerializer.Serialize(loginTestData)}");
    }

    [Test, TestCaseSource(nameof(GetSearchTestData))]
    public void TC02_SearchHotel(SearchTestData searchTestData)
    {
        _loginPage.Login(searchTestData.Url, searchTestData.Username, searchTestData.Password);
        _searchPage.SearchHotel(searchTestData.City, searchTestData.Hotel, searchTestData.RoomType,
            searchTestData.NoOfRoom, searchTestData.CheckInDate, searchTestData.CheckOutDate, searchTestData.Adults,
            searchTestData.Child);
        //By span = GetLocator(searchTestData.Result.LocatorType, searchTestData.Result.LocatorValue);
        //Globals.WaitForElement(span);
        //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        //IWebElement spanElement = wait.Until(ExpectedConditions.ElementIsVisible(span));
        var actualMessage = GetText(searchTestData.Result.LocatorType, searchTestData.Result.LocatorValue,
            searchTestData.Result.TextSource);
        Assert.That(actualMessage, Is.EqualTo(searchTestData.Result.ExpectedMessage));
        Test.Log(Status.Info, $"Test Data: {JsonSerializer.Serialize(searchTestData)}");
    }

    [Test, TestCaseSource(nameof(GetSearchTestData))]
    public void TC03_SelectHotel(SearchTestData searchTestData)
    {
        _loginPage.Login(searchTestData.Url, searchTestData.Username, searchTestData.Password);
        _searchPage.SearchHotel(searchTestData.City, searchTestData.Hotel, searchTestData.RoomType,
            searchTestData.NoOfRoom, searchTestData.CheckInDate, searchTestData.CheckOutDate, searchTestData.Adults,
            searchTestData.Child);
        if (!"Invalid".Equals(searchTestData.Type))
            _selectPage.SelectHotel();
        Test.Log(Status.Info, $"Test Data: {JsonSerializer.Serialize(searchTestData)}");
    }

    [Test, TestCaseSource(nameof(GetBookingTestData))]
    public void TC04_BookHotel(BookingTestData bookingTestData)
    {
        _loginPage.Login(bookingTestData.Url, bookingTestData.Username, bookingTestData.Password);
        _searchPage.SearchHotel(bookingTestData.City, bookingTestData.Hotel, bookingTestData.RoomType,
            bookingTestData.NoOfRoom, bookingTestData.CheckInDate, bookingTestData.CheckOutDate, bookingTestData.Adults,
            bookingTestData.Child);
        _selectPage.SelectHotel();
        _bookingPage.BookHotel(bookingTestData.FirstName, bookingTestData.LastName, bookingTestData.Address,
            bookingTestData.CreditCardNo, bookingTestData.CreditCardType, bookingTestData.ExpiryDateMonth,
            bookingTestData.ExpiryDateYear, bookingTestData.Cvv);

        //By myItinerary = GetLocator(bookingTestData.Result.LocatorType, bookingTestData.Result.LocatorValue);
        //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        //IWebElement myItineraryElement = wait.Until(ExpectedConditions.ElementIsVisible(myItinerary));
        var actualMessage = GetText(bookingTestData.Result.LocatorType, bookingTestData.Result.LocatorValue,
            bookingTestData.Result.TextSource);
        Assert.That(actualMessage, Is.EqualTo(bookingTestData.Result.ExpectedMessage));
        Test.Log(Status.Info, $"Test Data: {JsonSerializer.Serialize(bookingTestData)}");
    }

    private static IEnumerable<LoginTestData> GetLoginTestData()
    {
        var json = File.ReadAllText("TestData.json");
        var testData = JsonSerializer.Deserialize<Dictionary<string, List<LoginTestData>>>(json);

        if (testData == null || !testData.TryGetValue("Logins", out var logins)) yield break;
        foreach (var login in logins)
        {
            yield return login;
        }
    }


    private static IEnumerable<SearchTestData> GetSearchTestData()
    {
        var json = File.ReadAllText("TestData.json");
        var testData = JsonSerializer.Deserialize<Dictionary<string, List<SearchTestData>>>(json);

        if (testData?.ContainsKey("Logins") != true ||
            !testData.ContainsKey("Searches") ||
            JsonSerializer.Deserialize<List<LoginTestData>>(JsonSerializer.Serialize(testData["Logins"])) is
                not { } logins ||
            JsonSerializer.Deserialize<List<RawSearchData>>(JsonSerializer.Serialize(testData["Searches"])) is
                not { } searches) yield break;
        foreach (var rawSearch in searches)
        {
            var loginDetails = logins[rawSearch.LoginRef];

            yield return new SearchTestData(
                Type: rawSearch.Type,
                Url: loginDetails.Url,
                Username: loginDetails.Username,
                Password: loginDetails.Password,
                City: rawSearch.City,
                Hotel: rawSearch.Hotel,
                RoomType: rawSearch.RoomType,
                NoOfRoom: rawSearch.NoOfRoom,
                CheckInDate: rawSearch.CheckInDate,
                CheckOutDate: rawSearch.CheckOutDate,
                Adults: rawSearch.Adults,
                Child: rawSearch.Child,
                Result: rawSearch.Result
            );
        }
    }

    private static IEnumerable<BookingTestData> GetBookingTestData()
    {
        var json = File.ReadAllText("TestData.json");
        var testData = JsonSerializer.Deserialize<Dictionary<string, List<BookingTestData>>>(json);

        if (testData?.ContainsKey("Logins") != true ||
            !testData.ContainsKey("Bookings") ||
            JsonSerializer.Deserialize<List<LoginTestData>>(JsonSerializer.Serialize(testData["Logins"])) is
                not { } logins ||
            JsonSerializer.Deserialize<List<RawBookingData>>(JsonSerializer.Serialize(testData["Bookings"])) is
                not { } bookings) yield break;
        foreach (var rawBooking in bookings)
        {
            var loginDetails = logins[rawBooking.LoginRef];

            yield return new BookingTestData(
                Type: rawBooking.Type,
                Url: loginDetails.Url,
                Username: loginDetails.Username,
                Password: loginDetails.Password,
                City: rawBooking.City,
                Hotel: rawBooking.Hotel,
                RoomType: rawBooking.RoomType,
                NoOfRoom: rawBooking.NoOfRoom,
                CheckInDate: rawBooking.CheckInDate,
                CheckOutDate: rawBooking.CheckOutDate,
                Adults: rawBooking.Adults,
                Child: rawBooking.Child,
                FirstName: rawBooking.FirstName,
                LastName: rawBooking.LastName,
                Address: rawBooking.Address,
                CreditCardNo: rawBooking.CreditCardNo,
                CreditCardType: rawBooking.CreditCardType,
                ExpiryDateMonth: rawBooking.ExpiryDateMonth,
                ExpiryDateYear: rawBooking.ExpiryDateYear,
                Cvv: rawBooking.Cvv,
                Result: rawBooking.Result
            );
        }
    }
}