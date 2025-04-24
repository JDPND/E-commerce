using System.Diagnostics;
using Allure.Commons;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;



namespace Herokuapp.Support
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly IObjectContainer _container;
        private IWebDriver _driver;

        public Hooks(ScenarioContext scenarioContext, FeatureContext featureContext, IObjectContainer container)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _container = container;
        }

        [BeforeTestRun]
        public static void SetupAllure()
        {
            string resultsDir = Path.Combine(Directory.GetCurrentDirectory(),
                                          "bin", "Debug", "net6.0", "allure-results");

            // Ensure directory exists
            Directory.CreateDirectory(resultsDir);

            // Set environment variable
            Environment.SetEnvironmentVariable("ALLURE_RESULTS_DIRECTORY", resultsDir);

            Console.WriteLine($"Allure results will be saved to: {resultsDir}");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");
            options.AddArgument("--incognito");
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);

            _driver = new ChromeDriver(options);  
            _driver.Manage().Window.Maximize();

            _container.RegisterInstanceAs<IWebDriver>(_driver);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            if (context.TestError != null)
            {
                TakeScreenshot(_container.Resolve<IWebDriver>(), context.ScenarioInfo.Title);
            }
        }

        private void TakeScreenshot(IWebDriver driver, string scenarioTitle)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var bytes = screenshot.AsByteArray;
                AllureLifecycle.Instance.AddAttachment(
                    $"Screenshot_{scenarioTitle}_{DateTime.Now:yyyyMMddHHmmss}",
                    "image/png",
                    bytes
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                string allureResults = Path.Combine(AppContext.BaseDirectory, "allure-results");

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c allure serve \"{allureResults}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                var process = Process.Start(startInfo);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("✅ Allure CLI Output:\n" + output);
                Console.WriteLine("❌ Allure CLI Errors:\n" + error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to run Allure CLI: {ex.Message}");
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver?.Quit();
        }
    }
}
