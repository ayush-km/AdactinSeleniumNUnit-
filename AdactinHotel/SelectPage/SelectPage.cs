using OpenQA.Selenium;

namespace AdactinSeleniumNUnit.AdactinHotel.SelectPage;

public class SelectPage : BasePage
{
    #region Locators

    private readonly By _selectRadioBtn = By.Id("radiobutton_0");
    private readonly By _continueBtn = By.Id("continue");

    #endregion

    public void SelectHotel()
    {
        Step = Test.CreateNode("SelectPage");
        Click(_selectRadioBtn);
        Click(_continueBtn);
    }
}