using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;


namespace VWOUIAutomation.Factories
{
    // Factory class for creating ChromeDriver instances with customizable options.
    public class ChromeDriverFactory
    {
        // Path to the ChromeDriver executable
        public string ChromeDriverPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        // Timeout for WebDriver operations
        public TimeSpan WebDriverTimeout { get; set; } = TimeSpan.FromSeconds(60);

        // Whether to run Chrome in incognito mode
        public bool Incognito { get; set; } = false;

        // Whether to run Chrome in headless mode (no GUI)
        public bool Headless { get; set; } = false;

        // Whether to disable the GPU for Chrome
        public bool DisableGpu { get; set; } = false;

        // Whether to run Chrome without a sandbox
        public bool NoSandBox { get; set; } = false;

        // Whether to start Chrome maximized
        public bool StartMaximized { get; set; } = true;

        // Additional command-line arguments for Chrome
        public IEnumerable<string> AdditionalArguments { get; set; } = new List<string>();

        // Additional user profile preferences for Chrome
        public IDictionary<string, string> AdditionalUserProfilePreferences { get; set; } = new Dictionary<string, string>();

        // Dependency injection container for registering WebDriver instance
        private readonly IObjectContainer objectContainer;

        // Path to the folder where downloads should be saved (null for default)
        public string DownloadsFolderPath { get; set; } = null;


        // Initializes a new instance of the ChromeDriverFactory class.    
        // Dependency injection container.
        public ChromeDriverFactory(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        // Creates a new instance of ChromeDriver with the specified options 
        // and return the created ChromeDriver instance..
        public IWebDriver Create(IObjectContainer objectContainer)
        {
            IWebDriver driver;
            var options = new ChromeOptions();
            var arguments = new List<string>();

            if (!string.IsNullOrEmpty(DownloadsFolderPath))
            {
                options.AddUserProfilePreference("download.default_directory", DownloadsFolderPath);
            }

            if (Incognito)
            {
                arguments.Add("incognito");
            }

            if (StartMaximized)
            {
                arguments.Add("start-maximized");
            }

            if (Headless)
            {
                arguments.Add("headless");
            }

            if (DisableGpu)
            {
                arguments.Add("disable-gpu");
            }

            if (NoSandBox)
            {
                arguments.Add("no-sandbox");
            }

            if (AdditionalArguments != null)
            {
                arguments = arguments.Union(AdditionalArguments).ToList();
            }

            options.AddArguments(arguments);

            if (AdditionalUserProfilePreferences != null)
            {
                foreach (var additionalUserProfilePreference in AdditionalUserProfilePreferences)
                {
                    options.AddUserProfilePreference(additionalUserProfilePreference.Key,
                        additionalUserProfilePreference.Value);
                }
            }

            options.UnhandledPromptBehavior = UnhandledPromptBehavior.Accept;
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            options.LeaveBrowserRunning = false;

            var chromeDriverService = ChromeDriverService.CreateDefaultService(ChromeDriverPath);
            chromeDriverService.HideCommandPromptWindow = true;

            driver = new ChromeDriver(chromeDriverService, options, WebDriverTimeout);
            objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            return driver;
        }
    }
}