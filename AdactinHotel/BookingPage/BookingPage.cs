using OpenQA.Selenium;

namespace AdactinSeleniumNUnit.AdactinHotel.BookingPage;

public class BookingPage : BasePage
{
    #region Locators

    private readonly By _firstNameTxt = By.Id("first_name");
    private readonly By _lastNameText = By.Id("last_name");
    private readonly By _addressTxt = By.Id("address");
    private readonly By _ccNoTxt = By.Id("cc_num");
    private readonly By _ccTypeDropDown = By.Id("cc_type");
    private readonly By _expiryMonthDropDown = By.Id("cc_exp_month");
    private readonly By _expiryYearDropDown = By.Id("cc_exp_year");
    private readonly By _cvvNoTxt = By.Id("cc_cvv");
    private readonly By _bookNowBtn = By.Id("book_now");

    #endregion


    public void BookHotel(string firstName,
        string lastName,
        string address,
        string creditCardNumber,
        string creditCardType,
        string expiryMonth,
        string expiryYear,
        string cvv)
    {
        Step = Test.CreateNode("BookingPage");
        Write(_firstNameTxt, firstName);
        Write(_lastNameText, lastName);
        Write(_addressTxt, address);
        Write(_ccNoTxt, creditCardNumber);
        Write(_ccTypeDropDown, creditCardType);
        Write(_expiryMonthDropDown, expiryMonth);
        Write(_expiryYearDropDown, expiryYear);
        Write(_cvvNoTxt, cvv);

        Driver.FindElement(_bookNowBtn).Click();
    }
}