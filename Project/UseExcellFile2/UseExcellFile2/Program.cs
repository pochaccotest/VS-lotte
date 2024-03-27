using NUnit.Framework;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[TestFixture]
public class LoginPageTest
{
    //define varfor start
    private IWebDriver driver;
    string filePath = @"..\..\..\..\TestData\login.xlsx";

    [SetUp]
    public void Setup()
    {
        var chromeDriverDirectory = @"D:\Duyen\VisualStudio\chromedriver.exe";
        var chromeOptions = new ChromeOptions();
        chromeOptions.BinaryLocation = @"D:\Duyen\VisualStudio\GoogleChromePortable64\App\Chrome-bin\Chrome.exe";

        driver = new ChromeDriver(chromeDriverDirectory, chromeOptions);
    }

    [Test]
    public void VerifyLogin()
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[1]; // assuming you want to read the first worksheet(0), (1) sheet 2

            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                Console.WriteLine($"Processing row: {row}");

                string username = worksheet.Cells[row, 1].Value.ToString(); // assuming usernames are in the first column
                string password = worksheet.Cells[row, 2].Value.ToString(); // assuming passwords are in the second column
                string vMsg = worksheet.Cells[row, 3].Value.ToString();
                string imgfile = worksheet.Cells[row, 4].Value.ToString();

                driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/web/index.php/auth/login");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                IWebElement usernameField = wait.Until(driver => driver.FindElement(By.Name("username")));
                IWebElement passwordField = wait.Until(driver => driver.FindElement(By.Name("password")));
                IWebElement loginButton = wait.Until(driver => driver.FindElement(By.ClassName("orangehrm-login-button")));

                usernameField.SendKeys(username);
                passwordField.SendKeys(password);
                loginButton.Click();

                // write a fucntion to verify displayed msg same with expected result
                IsOK(vMsg,imgfile);

                Thread.Sleep(2000);
                driver.Navigate().Refresh();
            }
        }
    }

    // fucntion to verify text
    private void IsOK(string vMsg, string imgfile)
    {
        //wait website a text Invalid credentials is displayed after login failed
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        try
        {
            // find an element invalid
            IWebElement MsgInva = driver.FindElement(By.ClassName("oxd-text oxd-text--p oxd-alert-content-text"));
            // store text of an element in a string
            vMsg = MsgInva.Text;
            if (vMsg.Contains("Invalid credentials"))
            {
                Thread.Sleep(2000);
                // Take a screenshot and save it to a file
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile($"..\\..\\..\\..\\TestResult\\{imgfile}.png");
            }
        }
        catch
        { }
        try
        {
            // find an element require username
            IWebElement MsgRqU = driver.FindElement(By.XPath("//div[@class='orangehrm-login-slot-wrapper']//div[1]//div[1]//span[1]"));
            // store text of an element in a string
            vMsg = MsgRqU.Text;
            if (vMsg.Contains("Required"))
            {
                Thread.Sleep(2000);
                // Take a screenshot and save it to a file
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile($"..\\..\\..\\..\\TestResult\\{imgfile}.png");
            }
        }
        catch { }
        try
        {
            // find an element require password
            IWebElement MsgRqP = driver.FindElement(By.XPath("//div[@class='orangehrm-login-form']//div[2]//div[1]//span[1]"));
            // store text of an element in a string
            vMsg = MsgRqP.Text;
            if (vMsg.Contains("Required"))
            {
                Thread.Sleep(2000);
                // Take a screenshot and save it to a file
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile($"..\\..\\..\\..\\TestResult\\{imgfile}.png");
            }
        }
        catch { }
        try
        {
            // find an element require password
            Thread.Sleep(2000);
            driver.Url.Contains(vMsg);            
            // Take a screenshot and save it to a file
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile($"..\\..\\..\\..\\TestResult\\{imgfile}.png");
        }
        catch { }
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}