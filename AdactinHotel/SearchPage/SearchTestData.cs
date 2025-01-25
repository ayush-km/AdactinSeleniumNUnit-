namespace AdactinSeleniumNUnit.AdactinHotel.SearchPage;

public record SearchTestData(
    string Type,
    string Url,
    string Username,
    string Password,
    string City,
    string Hotel,
    string RoomType,
    string NoOfRoom,
    string CheckInDate,
    string CheckOutDate,
    string Adults,
    string Child,
    Result Result
)
{
    public override string ToString()
    {
        return $"{Type} Search";
    }
}

public record RawSearchData(
    string Type,
    int LoginRef,
    string City,
    string Hotel,
    string RoomType,
    string NoOfRoom,
    string CheckInDate,
    string CheckOutDate,
    string Adults,
    string Child,
    Result Result
);