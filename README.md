
# 🛒 E-commerce Automation Testing Project

This repository contains an automated testing suite for an E-commerce site (**SauceDemo**) using **SpecFlow**, **Selenium WebDriver**, **xUnit**, and **Allure Reports**. The project is built on .NET 6 and follows a BDD (Behavior Driven Development) approach for end-to-end test automation.

## 🚀 Features

- Automated test execution for end-to-end user journeys
- BDD test definitions with SpecFlow and Gherkin
- Rich visual reports using Allure
- Screenshot capture on failure
- WebDriverManager for automatic browser driver management

## 🧰 Tech Stack

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SpecFlow](https://specflow.org/)
- [Selenium WebDriver](https://www.selenium.dev/)
- [xUnit](https://xunit.net/)
- [Allure Reports](https://docs.qameta.io/allure/)
- [WebDriverManager](https://github.com/rosolko/WebDriverManager.Net)

## 📁 Project Structure

```
E-commerce/
│
├── allure-report/           # Generated Allure HTML report
├── bin/                     # Output binaries
├── obj/                     # Intermediate build files
├── Features/                # SpecFlow feature files (BDD scenarios)
├── StepDefinitions/         # Step definition methods
├── Pages/                   # Page Object Model classes
├── Support/                 # Hooks and helpers
├── specflow.json            # SpecFlow configuration
├── Ecommerce.csproj         # Project file
└── run_allure.sh            # Script to generate Allure report
```

## 🧪 Test Scenario

### ✅ End-to-End Shopping Flow on SauceDemo

- Launch the SauceDemo site
- Validate login page elements
- Attempt login with invalid credentials and verify error message
- Log in with valid credentials
- Sort products by “Price (low to high)”
- Add two items to the cart
- Verify the cart badge shows count `2`
- Go to the cart page and verify both items are listed
- Remove one item and validate removal
- Proceed to checkout and fill personal details
- Verify order summary and complete the purchase
- Check the confirmation message
- Return to the products page and validate navigation

## 🛠️ Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/ecommerce-automation.git
   cd ecommerce-automation/E-commerce
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run Tests**
   ```bash
   dotnet test
   ```

4. **Generate Allure Report**
   Make sure Allure CLI is installed, then run:
   ```bash
   ./run_allure.sh
   ```

   Or manually:
   ```bash
   allure generate bin/Debug/net6.0/allure-results --clean -o allure-report
   allure open allure-report
   ```

## 📸 Screenshots on Failure

Failures automatically capture screenshots that are attached to the Allure report, helping in visual debugging.

## 🌱 Future Enhancements

- ✅ **Cross-browser testing** with Chrome, Firefox, and Edge
- ✅ **CI/CD integration** (e.g., GitHub Actions, Azure Pipelines)
- ✅ **Headless browser mode** for faster, CI-friendly execution
- ✅ **Data-driven testing** using external JSON/CSV/Excel sources
- ✅ **Scenario Outlines** for parameterized tests in Gherkin
- ✅ **Retry mechanism** for flaky tests
- ✅ **Docker containerization** for consistent execution
- ✅ **Tag-based test execution** using SpecFlow tags
- ✅ **Enhanced Allure integration**, including:
  - `@allure.label` for categorization
  - `@allure.severity` (e.g., blocker, critical, minor)
  - `@allure.description` for test step context
  - `@allure.link` for test case references (e.g., Jira, TestRail)
- ✅ **Categorized reporting** (e.g., smoke, regression, checkout tests)
