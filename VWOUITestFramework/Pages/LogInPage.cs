using OpenQA.Selenium;
using VWOUIAutomation.Hooks;

namespace VWOUIAutomation.Pages
{
    public class LogInPage
    {
        private readonly IWebDriver driver;
        private readonly string usernameData = EnvironmentData.email;
        private readonly string passwordData = EnvironmentData.Password;
        string blankData = " ";

        private readonly By username = By.CssSelector("input[name='username']");
        private readonly By usernameErrorMessage = By.CssSelector("input[name='txtUsername-error']");
        private readonly By password = By.CssSelector("input[name='password']");
        private readonly By passwordErrorMessage = By.CssSelector("input[name='txtPassword-error']");
        private readonly By rememberMeCheckbox =  By.XPath("//*[@id='checkbox - remember']");
        private readonly By loginButton = By.Id("js-login-btn");
        private readonly By invalidCredentialsErrorMessage = By.CssSelector("div[id='js-notification-box-msg']");

        public LogInPage(IWebDriver _driver)
        {
            driver = _driver;
        }


        public void FillValidUsername()
        {
            // driver.Click(username);
            driver.ClearAndSendKeys(username, usernameData);
        }

        public void FillValidPassword()
        {
            // driver.Click(password);
            driver.ClearAndSendKeys(password, passwordData);
        }

        public void FillBlankUsername()
        {
            driver.ClearAndSendKeys(password, blankData);
        }

        public void FillBlankPassword()
        {
            driver.ClearAndSendKeys(password, blankData);
        }

        public void ClickLoginButton()
        {
            driver.Click(loginButton);
        }

        public void FillInValidUsername()
        {
            driver.ClearAndSendKeys(password, WebDriverExtension.RandomUsername());
        }

        public void FillInValidPassword()
        {
            driver.ClearAndSendKeys(password, WebDriverExtension.RandomPassword());
        }

        public string VerifyUserLogin()
        {
            return driver.Url;
        }

        public void CheckRememberMeCheckBox()
        {
            driver.Click(rememberMeCheckbox);
        }

        public string ReturnInvalidCredentialsErrorMessage()
        {
            return driver.GetElementText(invalidCredentialsErrorMessage);
        }

        public string ReturnUserNameErrorMessage()
        {
            return driver.GetElementText(usernameErrorMessage);
        }

        public string ReturnPassswordErrorMessage()
        {
            return driver.GetElementText(passwordErrorMessage);
        }
    }
}
