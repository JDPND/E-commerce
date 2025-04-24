using OpenQA.Selenium;
using Xunit;

namespace Ecommerce.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver) => _driver = driver;

        public IWebElement UsernameInput => _driver.FindElement(By.Id("user-name"));
        public IWebElement PasswordInput => _driver.FindElement(By.Id("password"));
        public IWebElement LoginButton => _driver.FindElement(By.Id("login-button"));
        public IWebElement ErrorMessage => _driver.FindElement(By.CssSelector("[data-test='error']"));

        public IWebElement OkPopup => _driver.FindElement(By.XPath("//*[text()='OK']")); // fixed closing quote

        public LoginPage AreLoginElementsDisplayed()
        {
            Assert.True(UsernameInput.Displayed, "Username input is not visible");
            Assert.True(PasswordInput.Displayed, "Password input is not visible");
            Assert.True(LoginButton.Displayed, "Login button is not visible");
            return this;
        }

        public LoginPage EnterUsername(string username)
        {
            UsernameInput.Clear();
            UsernameInput.SendKeys(username);
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
            return this;
        }

        public ProductsPage ClickLogin()
        {
            LoginButton.Click();
            return new ProductsPage(_driver);
        }

        public LoginPage IsLoginErrorDisplayed()
        {
            Assert.True(ErrorMessage.Displayed, "Expected error message to be displayed when login fails.");
            return this;
        }
    }
}
