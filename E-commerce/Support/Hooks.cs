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
                // Go up to project root
                string baseDir = AppContext.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\..\.."));

                string resultsPath = Path.Combine(projectRoot, "bin", "Debug", "net6.0", "allure-results");
                string reportPath = Path.Combine(projectRoot, "Report");

                Console.WriteLine($"📁 Project root: {projectRoot}");
                Console.WriteLine($"📂 Allure results path: {resultsPath}");
                Console.WriteLine($"📄 Final Report path: {reportPath}");

                if (!Directory.Exists(resultsPath))
                {
                    Console.WriteLine("❌ Allure results folder not found.");
                    return;
                }

                Directory.CreateDirectory(reportPath);

                string script = $"allure generate \"{resultsPath}\" --single-file --clean -o \"{reportPath}\"";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"{script}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    Console.WriteLine("✅ Allure CLI Output:\n" + output);
                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine("❌ Allure CLI Error:\n" + error);
                        return;
                    }
                }

                // Automatically open the report in the default browser
                string reportFilePath = Path.Combine(reportPath, "index.html");
                if (File.Exists(reportFilePath))
                {
                    Process.Start(new ProcessStartInfo(reportFilePath) { UseShellExecute = true });
                    Console.WriteLine($"🌐 Report opened: {reportFilePath}");
                }
                else
                {
                    Console.WriteLine("⚠️ Report file not found to open.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed during Allure report generation or opening: {ex.Message}");
            }
        }



        [AfterScenario]
        public void AfterScenario()
        {
            _driver?.Quit();
        }
    }
}
