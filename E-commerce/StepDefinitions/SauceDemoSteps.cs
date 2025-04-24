using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Allure.Commons;
using Allure.SpecFlowPlugin;
using Allure.Xunit.Attributes;
using Xunit;
using FluentAssertions;

namespace Ecommerce.StepDefinitions
{
    [Binding]
    [AllureSuite("SauceDemo E2E Tests")]
    [AllureFeature("Complete Shopping Workflow")]
    [AllureOwner("Jaideep")]
    public class SauceDemoSteps
    {
        private readonly IWebDriver _driver;
        private Pages.LoginPage _loginPage;
        private Pages.ProductsPage _productsPage;
        private Pages.CartPage _cartPage;
        private Pages.CheckoutPage _checkoutPage;
        private Pages.CheckoutOverviewPage _overviewPage;
        private Pages.CheckoutCompletePage _completePage;

        public SauceDemoSteps(IWebDriver driver)
        {
            _driver = driver;
            _loginPage = new Pages.LoginPage(_driver);
        }

        [Given(@"I launch the SauceDemo site")]
        public void GivenILaunchTheSauceDemoSite()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Then(@"I validate the login page elements")]
        public void ThenIValidateTheLoginPageElements()
        {
            _loginPage.AreLoginElementsDisplayed();
        }

        [When(@"I try logging in with invalid credentials")]
        public void WhenITryLoggingInWithInvalidCredentials()
        {
            _loginPage
                .EnterUsername("test")
                .EnterPassword("wrongpassword")
                .ClickLogin(); // returns ProductsPage but we expect failure
        }

        [Then(@"I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            _loginPage.IsLoginErrorDisplayed();
        }

        [When(@"I log in with valid credentials")]
        public void WhenILogInWithValidCredentials()
        {
            _productsPage = _loginPage
                .EnterUsername("standard_user")
                .EnterPassword("secret_sauce")
                .ClickLogin();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(driver => driver.FindElement(By.ClassName("title")).Displayed);
        }

        [When(@"I sort products by ""(.*)""")]
        public void WhenISortProductsBy(string sortOption)
        {
            _productsPage.SortBy(sortOption);
        }

        [When(@"I add two items to the cart")]
        public void WhenIAddTwoItemsToTheCart()
        {
            _productsPage.AddFirstNItemsToCart(2);
        }

        [Then(@"I verify the cart badge count is (.*)")]
        public void ThenIVerifyTheCartBadgeCountIs(int expectedCount)
        {
            _productsPage.GetCartBadgeCount().Should().Be(expectedCount);
        }

        [When(@"I go to the cart page")]
        public void WhenIGoToTheCartPage()
        {
            _cartPage = _productsPage.ClickCartIcon();
        }

        [Then(@"I verify both items are listed")]
        public void ThenIVerifyBothItemsAreListed()
        {
            _cartPage.GetCartItemCount().Should().Be(2);
        }

        [When(@"I remove one item from the cart")]
        public void WhenIRemoveOneItemFromTheCart()
        {
            _cartPage.RemoveFirstItem();
        }

        [Then(@"I verify the item is removed")]
        public void ThenIVerifyTheItemIsRemoved()
        {
            _cartPage.GetCartItemCount().Should().Be(1);
        }

        [When(@"I proceed to checkout and fill in personal details")]
        public void WhenIProceedToCheckoutAndFillInPersonalDetails()
        {
            _checkoutPage = _cartPage.ClickCheckout();
            _overviewPage = _checkoutPage
                .EnterFirstName("Test")
                .EnterLastName("User")
                .EnterZipCode("123456")
                .ContinueCheckout();
        }

        [Then(@"I verify the order summary and finish the purchase")]
        public void ThenIVerifyTheOrderSummaryAndFinishThePurchase()
        {
            _overviewPage.IsItemTotalDisplayed().Should().BeTrue();
            _overviewPage.IsTaxDisplayed().Should().BeTrue();
            _overviewPage.IsTotalDisplayed().Should().BeTrue();

            _completePage = _overviewPage.FinishOrder();
        }

        [Then(@"I should see the confirmation message")]
        public void ThenIShouldSeeTheConfirmationMessage()
        {
            _completePage.GetConfirmationMessage().Should().Be("Thank you for your order!");
        }

        [When(@"I click back to products")]
        public void WhenIClickBackToProducts()
        {
            _productsPage = _completePage.ClickBackToHome();
        }

        [Then(@"I should land on the products page")]
        public void ThenIShouldLandOnTheProductsPage()
        {
            _productsPage.GetPageTitle().Should().Be("Products");
        }
    }
}
