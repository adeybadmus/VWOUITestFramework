using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;

namespace VWOUIAutomation.Factories
{
    // Factory class for creating InternetExplorerDriver instances with customizable options.
    public class InternetExplorerDriverFactory
    {
        // Dependency injection container for registering WebDriver instance
        private readonly IObjectContainer objectContainer;

        // Path to the Internet Explorer Driver executable
        public string IEDriverPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        /// Initializes a new instance of the InternetExplorerDriverFactory class.
        public InternetExplorerDriverFactory(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        // Creates a new instance of InternetExplorerDriver with the specified options 
        // and return the created InternetExplorerDriver instance.
        public IWebDriver Create(IObjectContainer objectContainer)
        {
            IWebDriver driver;

            // Configure InternetExplorerOptions
            var options = new InternetExplorerOptions
            {
                IgnoreZoomLevel = true
            };

            // Create InternetExplorerDriver instance with specified options
            driver = new InternetExplorerDriver(IEDriverPath, options, TimeSpan.FromSeconds(60));

            // Register the WebDriver instance with the dependency injection container
            objectContainer.RegisterInstanceAs<IWebDriver>(driver);

            // Return the created InternetExplorerDriver instance
            return driver;
        }
    }
}
