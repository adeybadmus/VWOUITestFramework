using OpenQA.Selenium;

namespace VWOUIAutomation.Factories
{
    // Interface for WebDriver factory classes responsible for creating WebDriver instances.
    public interface IWebDriverFactory
    {
        // Creates a new instance of the WebDriver and return the created WebDriver instance.
        IWebDriver Create();
    }
}