namespace AdactinSeleniumNUnit.AdactinHotel.BookingPage;

public record BookingTestData(
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
    string FirstName,
    string LastName,
    string Address,
    string CreditCardNo,
    string CreditCardType,
    string ExpiryDateMonth,
    string ExpiryDateYear,
    string Cvv,
    Result Result
)
{
    public override string ToString()
    {
        return $"{Type} Booking";
    }
}

public record RawBookingData(
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
    string FirstName,
    string LastName,
    string Address,
    string CreditCardNo,
    string CreditCardType,
    string ExpiryDateMonth,
    string ExpiryDateYear,
    string Cvv,
    Result Result
);