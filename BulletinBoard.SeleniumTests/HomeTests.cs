using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

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
            var nav = driver.Navigate();

            // Act
            nav.GoToUrl(url);
            var tableTitle = driver.FindElement(By.Id("popularOffersTitle")).Text;

            // Assert
            Assert.AreEqual("Popular job offers:", tableTitle);

            // Cleanup
            driver.Close();
            driver.Dispose();
        }

        [TestMethod]
        public void UserName_IsVisible_InNavbar_AfterLogin()
        {
            // Arrange
            var driver = new InternetExplorerDriver();
            var nav = driver.Navigate();
            var url = "http://localhost:5000/account/login";
            nav.GoToUrl(url);

            // Act
            var emailField = driver.FindElement(By.Id("Email"));
            emailField.Click();
            emailField.SendKeys("admin@admin.com");

            var passwordField = driver.FindElement(By.Id("Password"));
            passwordField.Click();
            passwordField.SendKeys("admin");

            var submitButton = driver.FindElement(By.Id("LoginButton"));
            submitButton.Click();

            // Assert
            var userGreet = driver.FindElement(By.Id("UserName")).Text;
            StringAssert.Contains(userGreet, "admin@admin.com");

            // Cleanup
            driver.Close();
            driver.Dispose();
        }

        [TestMethod]
        public void EmailError_Appears_WhenUserTriesToRegister_WithInvalidEmail()
        {
            // Arrange
            var driver = new InternetExplorerDriver();
            var nav = driver.Navigate();
            var url = "http://localhost:5000/account/register";
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

            var submitButton = driver.FindElement(By.Id("LoginButton"));
            submitButton.Click();

            // Assert
            var emailError = driver.FindElement(By.Id("Email-error")).Text;
            StringAssert.Contains(emailError, "not a valid e-mail address");

            // Cleanup
            driver.Close();
            driver.Dispose();
        }
    }
}
