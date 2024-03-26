using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

[TestFixture]
public class LoginPageTest : IDisposable
{
    private IWebDriver? driver;

    public object? ScreenshotImageFormat { get; private set; }

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
        driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/web/index.php/auth/login");

        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@placeholder='Username']")).SendKeys(" ");
        driver.FindElement(By.XPath("//input[@placeholder='Password']")).SendKeys(" ");
        driver.FindElement(By.XPath("//button[@type='submit']")).Click();

        Thread.Sleep(2000);
        //driver.FindElement(By.LinkText("My Info")).Click();
        try
        {
            // find an element invalid
            IWebElement MsgInva = driver.FindElement(By.ClassName("oxd-alert-content-text"));
            // store text of an element in a string
            String vMsgInva = MsgInva.Text;
            if (vMsgInva.Contains("Invalid credentials"))
            {
                Console.WriteLine($"Testcase Pass");
                Thread.Sleep(2000);
                // Take a screenshot and save it to a file
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"..\\..\\..\\..\\TestResult\\Invalid.png");
            }
        }
        catch
        {           
                // find an element require username
                IWebElement MsgRqU = driver.FindElement(By.XPath("//div[@class='orangehrm-login-form']//div[2]//div[1]//span[1]"));
                // store text of an element in a string
                String vMsgRqU = MsgRqU.Text;
                if (vMsgRqU.Contains("Required"))
                {
                    Thread.Sleep(2000);
                    // Take a screenshot and save it to a file
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"..\\..\\..\\..\\TestResult\\RequiredU.png");
                }
            
                // find an element require password
                IWebElement MsgRqP = driver.FindElement(By.XPath("//div[@class='orangehrm-login-slot-wrapper']//div[1]//div[1]//span[1]"));
                // store text of an element in a string
                String vMsgRqP = MsgRqP.Text;
                if (vMsgRqP.Contains("Required"))
                {
                    Thread.Sleep(2000);
                    // Take a screenshot and save it to a file
                    ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile($"..\\..\\..\\..\\TestResult\\RequiredP.png");
                }           
        }
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
