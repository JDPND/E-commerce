using OpenQA.Selenium;

namespace Ecommerce.Pages
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;

        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FirstName => _driver.FindElement(By.Id("first-name"));
        public IWebElement LastName => _driver.FindElement(By.Id("last-name"));
        public IWebElement ZipCode => _driver.FindElement(By.Id("postal-code"));
        public IWebElement ContinueButton => _driver.FindElement(By.Id("continue"));

        public CheckoutPage EnterFirstName(string firstName)
        {
            FirstName.Clear();
            FirstName.SendKeys(firstName);
            return this;
        }

        public CheckoutPage EnterLastName(string lastName)
        {
            LastName.Clear();
            LastName.SendKeys(lastName);
            return this;
        }

        public CheckoutPage EnterZipCode(string zipCode)
        {
            ZipCode.Clear();
            ZipCode.SendKeys(zipCode);
            return this;
        }

        public CheckoutOverviewPage ContinueCheckout()
        {
            ContinueButton.Click();
            return new CheckoutOverviewPage(_driver);
        }

        public CheckoutOverviewPage FillCheckoutDetails(string firstName, string lastName, string zipCode)
        {
            return EnterFirstName(firstName)
                .EnterLastName(lastName)
                .EnterZipCode(zipCode)
                .ContinueCheckout();
        }
    }
}
