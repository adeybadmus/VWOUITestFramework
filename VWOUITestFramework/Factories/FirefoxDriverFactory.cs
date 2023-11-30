using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace VWOUIAutomation.Factories
{

    // Factory class for creating FirefoxDriver instances with customizable options.
    public class FirefoxDriverFactory
    {
        // Dependency injection container for registering WebDriver instance
        private readonly IObjectContainer objectContainer;

        // Path to the GeckoDriver executable
        public string GeckoDriverPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        // Timeout for WebDriver operations
        public TimeSpan WebDriverTimeout { get; set; } = TimeSpan.FromSeconds(60);

        // Whether to run Firefox in headless mode (no GUI)
        public bool Headless { get; set; } = false;

        /// Initializes a new instance of the FirefoxDriverFactory class.
        public FirefoxDriverFactory(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        // Creates a new instance of FirefoxDriver with the specified options 
        // and return the created FirefoxDriver instance.
        public IWebDriver Create(IObjectContainer objectContainer)
        {
            IWebDriver driver;

            // Configure FirefoxOptions with a profile that is deleted after use
            var firefoxOptions = new FirefoxOptions
            {
                Profile = new FirefoxProfile
                {
                    DeleteAfterUse = true
                }
            };

            if (Headless)
            {
                // Add the "--headless" argument for running Firefox in headless mode
                firefoxOptions.AddArguments("--headless");
            }

            // Create FirefoxDriver instance with specified options
            driver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(GeckoDriverPath), firefoxOptions, WebDriverTimeout);

            // Maximize the browser window
            driver.Manage().Window.Maximize();

            // Register the WebDriver instance with the dependency injection container
            objectContainer.RegisterInstanceAs<IWebDriver>(driver);

            // Return the created FirefoxDriver instance
            return driver;
        }
    }
}

