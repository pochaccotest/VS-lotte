using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

public class LoginTest
{
    private IWebDriver driver;
    private string filePath = @"path\to\your\file.csv"; // Update with the path to your CSV file

    public LoginTest()
    {
        driver = new ChromeDriver();
    }

    public void ReadDataAndLogin()
    {
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                string username = values[0];
                string password = values[1];

                // Now use these credentials to login
                Login(username, password);
            }
        }
    }

    private void Login(string username, string password)
    {
        // Navigate to the login page
        driver.Navigate().GoToUrl("http://your-login-page.com");

        // Find the username field and enter the username
        driver.FindElement(By.Id("username")).SendKeys(username);

        // Find the password field and enter the password
        driver.FindElement(By.Id("password")).SendKeys(password);

        // Find the login button and click it
        driver.FindElement(By.Id("loginButton")).Click();
    }
}