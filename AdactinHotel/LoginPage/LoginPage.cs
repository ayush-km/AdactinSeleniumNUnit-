using OpenQA.Selenium;

namespace AdactinSeleniumNUnit.AdactinHotel.LoginPage;

public class LoginPage : BasePage
{
    private readonly By _usernameTxt = By.Id("username");
    private readonly By _passwordTxt = By.Id("password");
    private readonly By _loginBtn = By.Id("login");

    public void Login(string url, string username, string password)
    {
        Step = Test.CreateNode("LoginPage");
        OpenUrl(url);
        Write(_usernameTxt, username);
        Write(_passwordTxt, password);
        Click(_loginBtn);
    }
}