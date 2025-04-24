using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Allure.Commons;
using Allure.SpecFlowPlugin;
using Allure.Xunit.Attributes.Steps;
using Allure.Xunit.Attributes;

namespace Ecommerce.StepDefinitions
{
    [Binding]
    [AllureSuite("SauceDemo E2E Tests")]
    [AllureFeature("Complete Shopping Workflow")]
    [AllureOwner("Jaideep")]
    public class SauceDemoSteps
    {
        private readonly IWebDriver _driver;
        private readonly Pages.LoginPage _loginPage;
        private readonly Pages.ProductsPage _productsPage;
        private readonly Pages.CartPage _cartPage;
        private readonly Pages.CheckoutPage _checkoutPage;
        private readonly Pages.CheckoutOverviewPage _overviewPage;
        private readonly Pages.CheckoutCompletePage _completePage;

        public SauceDemoSteps(IWebDriver driver)
        {
            _driver = driver;
            _loginPage = new Pages.LoginPage(_driver);
            _productsPage = new Pages.ProductsPage(_driver);
            _cartPage = new Pages.CartPage(_driver);
            _checkoutPage = new Pages.CheckoutPage(_driver);
            _overviewPage = new Pages.CheckoutOverviewPage(_driver);
            _completePage = new Pages.CheckoutCompletePage(_driver);
        }

        [AllureStep("Launch the SauceDemo site")]
        [Given(@"I launch the SauceDemo site")]
        public void GivenILaunchTheSauceDemoSite()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [AllureStep("Validate the login page elements")]
        [Then(@"I validate the login page elements")]
        public void ThenIValidateTheLoginPageElements()
        {
            _loginPage.AreLoginElementsDisplayed();
        }

        [AllureStep("Try logging in with invalid credentials")]
        [When(@"I try logging in with invalid credentials")]
        public void WhenITryLoggingInWithInvalidCredentials()
        {
            _loginPage.Login("test", "test1234");
        }

        [AllureStep("Verify error message is displayed")]
        [Then(@"I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            _loginPage.IsLoginErrorDisplayed();
        }

        [AllureStep("Log in with valid credentials")]
        [AllureSeverity((Allure.Net.Commons.SeverityLevel)SeverityLevel.blocker)]
        [When(@"I log in with valid credentials")]
        public void WhenILogInWithValidCredentials()
        {
            _loginPage.Login("standard_user", "secret_sauce");
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.ClassName("title")).Displayed);
        }

        [AllureStep("Sort products by option: {0}")]
        [When(@"I sort products by ""(.*)""")]
        public void WhenISortProductsBy(string sortOption)
        {
            _productsPage.SortBy(sortOption);
        }

        [AllureStep("Add two items to the cart")]
        [When(@"I add two items to the cart")]
        public void WhenIAddTwoItemsToTheCart()
        {
            _productsPage.AddFirstNItemsToCart(2);
        }

        [AllureStep("Verify cart badge count is {0}")]
        [Then(@"I verify the cart badge count is (.*)")]
        public void ThenIVerifyTheCartBadgeCountIs(int expectedCount)
        {
            int actualCount = _productsPage.GetCartBadgeCount();
            Assert.Equal(expectedCount, actualCount);
        }

        [AllureStep("Navigate to the cart page")]
        [When(@"I go to the cart page")]
        public void WhenIGoToTheCartPage()
        {
            _productsPage.ClickCartIcon();
        }

        [AllureStep("Verify both items are listed in the cart")]
        [Then(@"I verify both items are listed")]
        public void ThenIVerifyBothItemsAreListed()
        {
            int itemCount = _cartPage.GetCartItemCount();
            Assert.Equal(2, itemCount);
        }

        [AllureStep("Remove one item from the cart")]
        [When(@"I remove one item from the cart")]
        public void WhenIRemoveOneItemFromTheCart()
        {
            _cartPage.RemoveFirstItem();
        }

        [AllureStep("Verify the item is removed from the cart")]
        [Then(@"I verify the item is removed")]
        public void ThenIVerifyTheItemIsRemoved()
        {
            int itemCount = _cartPage.GetCartItemCount();
            Assert.Equal(1, itemCount);
        }

        [AllureStep("Proceed to checkout and enter personal details")]
        [When(@"I proceed to checkout and fill in personal details")]
        public void WhenIProceedToCheckoutAndFillInPersonalDetails()
        {
            _cartPage.ClickCheckout();
            _checkoutPage.FillCheckoutDetails("test", "Demo", "123456");
        }

        [AllureStep("Verify order summary and finish purchase")]
        [Then(@"I verify the order summary and finish the purchase")]
        public void ThenIVerifyTheOrderSummaryAndFinishThePurchase()
        {
            Assert.True(_overviewPage.IsItemTotalDisplayed(), "Item Total is not displayed.");
            Assert.True(_overviewPage.IsTaxDisplayed(), "Tax is not displayed.");
            Assert.True(_overviewPage.IsTotalDisplayed(), "Total is not displayed.");
            _overviewPage.FinishOrder();
        }

        [AllureStep("Verify confirmation message after purchase")]
        [Then(@"I should see the confirmation message")]
        public void ThenIShouldSeeTheConfirmationMessage()
        {
            _completePage.GetConfirmationMessage().Should().Be("Thank you for your order!");
        }

        [AllureStep("Click back to products")]
        [When(@"I click back to products")]
        public void WhenIClickBackToProducts()
        {
            _completePage.ClickBackToHome();
        }

        [AllureStep("Verify navigation back to products page")]
        [Then(@"I should land on the products page")]
        public void ThenIShouldLandOnTheProductsPage()
        {
            _productsPage.GetPageTitle().Should().Be("Products");
        }
    }
}
