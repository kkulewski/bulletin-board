using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace BulletinBoard.SeleniumTests
{
    [TestClass]
    public class HomeTests
    {
        [TestMethod]
        public void Popular_Has_CorrectTableTitle()
        {
            // Arrange
            var driver = new InternetExplorerDriver();
            var url = "http://localhost:5000/";

            // Act
            try
            {
                var nav = driver.Navigate();
                nav.GoToUrl(url);
                var tableTitle = driver.FindElement(By.Id("popularOffersTitle")).Text;

                // Assert
                Assert.AreEqual("Popular job offers:", tableTitle);
            }
            catch(Exception e)
            {
                // Assert
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        [TestMethod]
        public void UserName_IsVisible_InNavbar_AfterLogin()
        {
            // Arrange
            var driver = new InternetExplorerDriver();
            var url = "http://localhost:5000/account/login";

            // Act
            try
            {
                var nav = driver.Navigate();
                nav.GoToUrl(url);
                var emailField = driver.FindElement(By.Id("Email"));
                emailField.Click();
                emailField.SendKeys("admin@admin.com");

                var passwordField = driver.FindElement(By.Id("Password"));
                passwordField.Click();
                passwordField.SendKeys("admin");

                var submitButton = driver.FindElement(By.Id("LoginButton"));
                submitButton.SendKeys("\n");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(UrlToBe("http://localhost:5000/"));
                var userGreet = driver.FindElement(By.Id("UserName")).Text;

                // Assert
                StringAssert.Contains(userGreet, "admin@admin.com");
            }
            catch(Exception e)
            {
                // Assert
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        [TestMethod]
        public void EmailError_Appears_WhenUserTriesToRegister_WithInvalidEmail()
        {
            // Arrange
            var driver = new InternetExplorerDriver();
            var url = "http://localhost:5000/account/register";

            try
            {
                var nav = driver.Navigate();
                nav.GoToUrl(url);

                // Act
                var emailField = driver.FindElement(By.Id("Email"));
                emailField.Click();
                emailField.SendKeys("wrong email");

                var passwordField = driver.FindElement(By.Id("Password"));
                passwordField.Click();
                passwordField.SendKeys("password");

                var confirmField = driver.FindElement(By.Id("ConfirmPassword"));
                confirmField.Click();
                confirmField.SendKeys("password");

                var submitButton = driver.FindElement(By.Id("RegisterButton"));
                submitButton.Click();

                var emailError = driver.FindElement(By.Id("Email-error")).Text;

                // Assert
                StringAssert.Contains(emailError, "not a valid e-mail address");
            }
            catch(Exception e)
            {
                // ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("path", ScreenshotImageFormat.Png);
                // Assert
                Assert.Fail(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        public static Func<IWebDriver, bool> UrlToBe(string url)
        {
            return (driver) => { return driver.Url.ToLowerInvariant().Equals(url.ToLowerInvariant()); };
        }
    }
}
