using NUnit.Framework;

namespace VWOUIAutomation
{
    // Provides access to environment-specific configuration data
    public class EnvironmentData
    {
        // Gets the base URL from the test context parameters
        public static string BaseUrl { get; } = TestContext.Parameters["baseURL"];

        // Gets the browser from the test context parameters
        public static string Browser { get; } = TestContext.Parameters["browser"];

        // Gets the database connection string from the test context parameters
        public static string ConnectionString { get; } = TestContext.Parameters["connectionString"];

        // Gets the directory for storing screenshots from the test context parameters
        public static string ScreenShotDirectory { get; } = TestContext.Parameters["ScreenShotDirectory"];

        public static string email { get; } = TestContext.Parameters["email"];

        public static string Password { get; } = TestContext.Parameters["password"];
    }
}

