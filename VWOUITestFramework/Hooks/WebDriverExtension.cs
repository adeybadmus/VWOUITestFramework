using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VWOUIAutomation.Hooks
{
    // Static class containing extension methods for IWebDriver.
    public static class WebDriverExtension
    {
        private static string _screenShotDirectory;
        private static readonly Random random;
        private static readonly TimeSpan implicitTimeout;
        private static readonly int timeout;


        // Extension method to find a web element with a specified timeout.        
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        // Method to get the screenshot directory.
        public static string GetScreenshotDirectory()
        {
            string screenShotDirectoryConfigValue = EnvironmentData.ScreenShotDirectory;
            _screenShotDirectory = null;

            if (!String.IsNullOrWhiteSpace(screenShotDirectoryConfigValue))
            {
                if (!screenShotDirectoryConfigValue.EndsWith(@"\"))
                {
                    screenShotDirectoryConfigValue += @"\";
                }
                _screenShotDirectory = screenShotDirectoryConfigValue;
            }
            return _screenShotDirectory;
        }

        // Method to generate a random number within a specified range.
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Method to generate a random text within a specified range.
        public static string RandomString(int size, bool lowerCase = false)
        {
            Random random = new Random();
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.

            // char is a single Unicode character
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26

            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static string RandomUsername()
        {
            var usernameBuilder = new StringBuilder();

            //// 1-Letters lower case
            //usernameBuilder.Append(RandomString(1));

            // 4-Letters lower case
            usernameBuilder.Append(RandomString(5, true));


            return usernameBuilder.ToString();
        }

        public static string RandomPassword()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }

        private static void setImplicitTimeout(this IWebDriver driver, int timeout)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
        }

        private static void switchOffImplicitWaiting(this IWebDriver driver)
        {
            setImplicitTimeout(driver, 0);
        }

        private static void resetImplicitTimeOut(this IWebDriver driver)
        {
            setImplicitTimeout(driver, timeout);
        }

        // Extension method to clear and send keys to a web element.
        internal static void ClearAndSendKeys(this IWebDriver driver, By identifier, string text, int? waitingTime = null)
        {
            driver.FindElement(identifier, 5).Clear();
            driver.FindElement(identifier).SendKeys(text);
            if (waitingTime != null)
            {
                Thread.Sleep(Convert.ToInt16(waitingTime));
            }
            driver.FindElement(identifier).SendKeys(Keys.Tab);
        }

        internal static void ClearAndSendKeysByElement(IWebElement element, string text, int? waitingTime = null)
        {
            element.Clear();
            element.SendKeys(text);
            if (waitingTime != null)
            {
                Thread.Sleep(Convert.ToInt16(waitingTime));
            }
            element.SendKeys(Keys.Tab);
        }

        internal static void SendKeys(this IWebDriver driver, By identifier, string text, int? waitingTime = null)
        {
            driver.FindElement(identifier, 5).SendKeys(text);
            if (waitingTime != null)
            {
                Thread.Sleep(Convert.ToInt16(waitingTime));
            }
            driver.FindElement(identifier).SendKeys(Keys.Tab);
        }

        internal static void SendKeysMultiple(this IWebDriver driver, By identifier, string text, int? waitingTime = null)
        {
            driver.FindElement(identifier, 5).SendKeys(text.Replace(" ", Environment.NewLine));
            if (waitingTime != null)
            {
                Thread.Sleep(Convert.ToInt16(waitingTime));
            }
            driver.FindElement(identifier).SendKeys(Keys.Tab);
        }

        internal static void Click(this IWebDriver driver, By identifier)
        {
            driver.FindElement(identifier, 5).Click();
        }

        internal static string GetUrl(this IWebDriver driver)
        {
            return driver.Url;
        }

        internal static void FocusAndClick(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            Actions act = new Actions(driver);
            act.MoveToElement(element).Click().Build().Perform();
        }

        internal static void Focus(this IWebDriver driver, By identifier = null, IWebElement webElement = null)
        {
            if (webElement != null)
            {
                Actions act = new Actions(driver);
                act.MoveToElement(webElement).Build().Perform();
            }
            else
            {
                IWebElement element = driver.FindElement(identifier, 5);
                Actions act = new Actions(driver);
                act.MoveToElement(element).Build().Perform();
            }
        }

        internal static void ScrollIntoView(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", element);
        }

        internal static bool SelectOptionByText(this IWebDriver driver, By identifier, string text)
        {
            bool elementToSelectExist = false;
            try
            {
                SelectElement select = new SelectElement(driver.FindElement(identifier, 5));
                Thread.Sleep(1000);
                select.SelectByText(text);
                elementToSelectExist = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return elementToSelectExist;
        }

        internal static void SelectOptionByValue(this IWebDriver driver, By identifier, string text)
        {
            SelectElement select = new SelectElement(driver.FindElement(identifier, 5));
            Thread.Sleep(1000);
            select.SelectByValue(text);
        }

        internal static string GetElementText(this IWebDriver driver, By identifier)
        {
            return driver.FindElement(identifier, 5).Text.Trim();
        }

        internal static void CloseCurrentWindow(this IWebDriver driver)
        {
            try
            {
                driver.Close();
                driver.Navigate().Refresh();
            }
            catch (Exception)
            {
                Console.WriteLine("Current Window has already been closed");
            }
        }

        internal static IList<IWebElement> FindElements(this IWebDriver driver, By Identifier)
        {
            return driver.FindElements(Identifier);
        }

        internal static IList<IWebElement> FindElements(this IWebDriver driver, By baseIdentifier, By subIdentifier)
        {
            return driver.FindElement(baseIdentifier).FindElements(subIdentifier);
        }

        internal static IWebElement FindElement(this IWebDriver driver, By Identifier)
        {
            return driver.FindElement(Identifier);
        }
        internal static ICollection<IWebElement> GetElementsList(this IWebDriver driver, By identifier)
        {
            ICollection<IWebElement> elementsInList = driver.FindElements(identifier);
            return elementsInList;
        }

        private static string GetText(IWebElement element)
        {

            return element.GetAttribute("value").Trim();

        }

        internal static List<string> GetTextFromAllElementsMatching(this IWebDriver driver, By identifier)
        {
            List<string> result = new List<string>();

            foreach (IWebElement element in driver.FindElements(identifier))
            {
                string elementText = GetText(element);
                result.Add(elementText);
            }

            return result;
        }

        internal static bool ElementIsNotDisplayed(this IWebDriver driver, By identifier)
        {
            return !driver.FindElement(identifier, 5).Displayed;
        }


        internal static bool ElementContainsText(this IWebDriver driver, By identifier, string expectedText)
        {
            List<string> values = GetTextFromAllElementsMatching(driver, identifier);
            foreach (string value in values)
            {
                if (value.ToLower().Contains(expectedText.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool ElementDoesNotContainText(this IWebDriver driver, By identifier, string expectedText)
        {
            List<string> values = GetTextFromAllElementsMatching(driver, identifier);
            foreach (string value in values)
            {
                if (value.ToLower().Contains(expectedText.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool ElementIsDisplayed(this IWebDriver driver, By identifier)
        {
            if (driver.FindElement(identifier, 5).Displayed)
            {
                return driver.FindElement(identifier).Displayed;
            }
            return false;
        }

        internal static bool ElementDisplayed(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);

            return element.Displayed;
        }


        internal static bool ElementExists(this IWebDriver driver, By identifier)
        {
            return ElementExists(driver, identifier, implicitTimeout);
        }

        internal static bool ElementExists(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            bool result = false;
            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, timeout);
                wait.Until(ExpectedConditions.ElementExists(identifier));
                // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(identifier));
                result = true;
            }
            catch (WebDriverTimeoutException)
            {
                result = false;
            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
            return result;
        }

        internal static bool ElementDoesNotExist(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(identifier));
            // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(identifier));
            bool result = true;
            try
            {
                Console.WriteLine("Element could not be found");

            }
            catch (WebDriverTimeoutException)
            {
                switchOffImplicitWaiting(driver);
            }
            finally
            {
                resetImplicitTimeOut(driver);
            }

            return result;
        }

        internal static bool WaitForElementToBeVisible(this IWebDriver driver, By identifier)
        {
            return WaitForElementToBeVisible(driver, identifier, implicitTimeout);
        }

        internal static bool WaitForElementToBeVisible(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            bool elementVisible = false;

            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, timeout);
                wait.Until(ExpectedConditions.ElementExists(identifier));
                wait.Until(ExpectedConditions.ElementIsVisible(identifier));
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(identifier));
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(identifier));

                elementVisible = true;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Element is not visible");

            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
            return elementVisible;
        }

        internal static void WaitForElementNotToBeVisible(this IWebDriver driver, By identifier)
        {
            WaitForElementNotToBeVisible(driver, identifier, implicitTimeout);
        }

        internal static void WaitForElementNotToBeVisible(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, timeout);
                wait.Until(ExpectedConditions.ElementIsVisible(identifier));
                // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(identifier));
            }
            catch (WebDriverTimeoutException)
            {

            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
        }
        internal static void WaitForElementToContainText(this IWebDriver driver, By identifier, string expectedText)
        {
            WaitForElementToContainText(driver, identifier, expectedText, implicitTimeout);
        }

        internal static void WaitForElementToContainText(this IWebDriver driver, By identifier, string expectedText, TimeSpan timeout)
        {
            try
            {
                switchOffImplicitWaiting(driver);
                WebDriverWait wait = new WebDriverWait(driver, timeout);
                wait.Until(elementContainsText(driver, identifier, expectedText));
            }
            catch (WebDriverTimeoutException)
            {

            }
            finally
            {
                resetImplicitTimeOut(driver);
            }
        }
        private static Func<IWebDriver, bool> elementContainsText(this IWebDriver driver, By identifier, String expectedText)
        {
            return (d) =>
            {
                bool result = false;
                try
                {
                    result = GetText(driver.FindElement(identifier, 5)).Contains(expectedText);
                }
                catch (StaleElementReferenceException)
                {
                    // Logger
                }

                return result;
            };
        }
        internal static void handleAlert(this IWebDriver driver)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.AlertIsPresent());
                // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                Thread.Sleep(TimeSpan.FromSeconds(5));
                alert.Accept();
            }
            catch (Exception)
            {
                Console.WriteLine("Continue with test execution");
            }
        }

        internal static void handleConfirmation(this IWebDriver driver)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.AlertIsPresent());
                // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                Thread.Sleep(TimeSpan.FromSeconds(5));
                alert.Dismiss();
            }
            catch (Exception)
            {
                Console.WriteLine("Continue with test execution");
            }
        }
        internal static bool elementIsEnabled(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            return driver.FindElement(identifier, 5).Enabled;
        }

        internal static void switchToIFrame(this IWebDriver driver, string frameId)
        {
            if (ElementExists(driver, By.Id(frameId)))
            {
                navigateToFrame(driver, By.Id(frameId));
            }
        }
        internal static bool navigateToFrame(this IWebDriver driver, By identifier)
        {
            bool navigationSuccessful = false;
            try
            {
                // Logger
                driver.SwitchTo().Frame(driver.FindElement(identifier, 5));
                navigationSuccessful = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Navigation to the frame was not successful");
            }

            return navigationSuccessful;
        }

        internal static void switchBackToDefaultContent(this IWebDriver driver)
        {
            driver.SwitchTo().DefaultContent();
        }

        internal static void refreshWindow(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        internal static void switchToDefaultWindow(this IWebDriver driver)
        {
            string currentWindow = driver.CurrentWindowHandle;
            driver.SwitchTo().Window(currentWindow);
        }

        internal static string switchToLastWindow(this IWebDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Manage().Window.Maximize();

            return driver.Title;
        }

        internal static void AcceptConfirmWindow(this IWebDriver driver)
        {
            driver.SwitchTo().Alert().Accept();
        }

        internal static void waitForPageToLoad(this IWebDriver driver, int second)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(second));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        internal static bool ElementIsSelected(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            return element.Selected;
        }

        internal static bool ElementIsClickable(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            return element.Enabled;
        }

        public static void SubmitFocus(this IWebDriver driver, By identifier)
        {
            Actions action = new Actions(driver);
            IWebElement element = driver.FindElement(identifier, 5);
            action.MoveToElement(element);
            Click(driver, identifier);
            action.Perform();
        }

        public static void pressTabKey(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            element.SendKeys(Keys.Tab);
        }

        public static void pressEnterKey(this IWebDriver driver, By identifier)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            element.SendKeys(Keys.Enter);
        }

        public static void enterTextInTextAreas(this IWebDriver driver, By identifier, string text, TimeSpan timeout)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            IList<IWebElement> elements = element.FindElements(identifier);

            foreach (IWebElement ele in elements)
            {
                ele.Clear();
                ele.SendKeys(text);
            }
        }

        public static string ChooseFromIdenticalElements(this IWebDriver driver, By identifier, TimeSpan timeout)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            IList<IWebElement> elements = element.FindElements(identifier);

            var option = elements[random.Next(elements.ToArray().Length)];
            string text = option.Text;
            Console.WriteLine("The text in the selected element is: [{0}]", text);
            option.Click();

            return text;
        }

        public static void ClickSpecificLinkFromIdenticalElements(this IWebDriver driver, By identifier, string textOflinkToClick, TimeSpan timeout)
        {
            IWebElement element = driver.FindElement(identifier, 5);
            IList<IWebElement> links = element.FindElements(identifier);

            foreach (var link in links)
            {
                if (link.Text.Contains(textOflinkToClick))
                {
                    link.Click();
                    break;
                }
            }
        }

        // Extension method to click on a specific tab identified by the given locator.
        public static void ClickTab(this IWebDriver driver, By tabLocator)
        {
            WaitForElementToBeVisible(driver, tabLocator);
            Click(driver, tabLocator);
        }

    }
}
