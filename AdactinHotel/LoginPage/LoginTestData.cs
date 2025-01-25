namespace AdactinSeleniumNUnit.AdactinHotel.LoginPage;

public record LoginTestData(
    string Type,
    string Url,
    string Username,
    string Password,
    Result Result)
{
    public override string ToString()
    {
        return $"{Type} Credential(s)";
    }
}