using OpenQA.Selenium;

namespace AdactinSeleniumNUnit.AdactinHotel.SearchPage;

public class SearchPage : BasePage
{
    #region Locators

    private readonly By _locationDropDown = By.Id("location");
    private readonly By _hotelDropdown = By.Id("hotels");
    private readonly By _roomTypeDropDown = By.Id("room_type");
    private readonly By _noOfRoomsDropDown = By.Id("room_nos");
    private readonly By _checkInDateTxt = By.Id("datepick_in");
    private readonly By _checkOutDateTxt = By.Id("datepick_out");
    private readonly By _adultPerRoomDropDown = By.Id("adult_room");
    private readonly By _childrenPerRoomDropDown = By.Id("child_room");
    private readonly By _searchBtn = By.Id("Submit");

    #endregion

    public void SearchHotel(string city,
        string hotelName,
        string roomType,
        string numberOfRooms,
        string checkInDate,
        string checkOutDate,
        string adultsPerRoom,
        string childrenPerRoom)
    {
        Step = Test.CreateNode("SearchPage");
        Write(_locationDropDown, city);
        Write(_hotelDropdown, hotelName);
        Write(_roomTypeDropDown, roomType);
        Write(_noOfRoomsDropDown, numberOfRooms);
        Write(_checkInDateTxt, checkInDate);
        Write(_checkOutDateTxt, checkOutDate);
        Write(_adultPerRoomDropDown, adultsPerRoom);
        Write(_childrenPerRoomDropDown, childrenPerRoom);

        Click(_searchBtn);
    }
}