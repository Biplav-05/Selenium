using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace erp_1.SeleniumTests;

public class ElementInteractionTests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
        // Load environment variables
        DotNetEnv.Env.TraversePath().Load();
        
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;

        bool isLocal = GetEnvVar("IS_LOCAL_SETUP").ToLower() == "true";
        string hubUrl = GetEnvVar("SEL_GRID_HUB_URL");

        if (isLocal)
        {
            _driver = new ChromeDriver(chromeOptions);
        }
        else
        {
            _driver = new RemoteWebDriver(new Uri(hubUrl), chromeOptions);
        }
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        
        // Adding implicit wait as shown in the Selenium documentation
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
    }

    [TearDown]
    public void Teardown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    [Test]
    public void ShouldHandleBasicElementInteractions()
    {
        // Navigate to the test page provided by Selenium documentation
        _driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/inputs.html");
        Thread.Sleep(2000); // Wait to visualize the documentation page

        // ==========================
        // 1. CLICK
        // ==========================
        Console.WriteLine("Testing Click...");
        // Click on the checkbox element
        IWebElement checkInput = _driver.FindElement(By.Name("checkbox_input"));
        checkInput.Click();
        Thread.Sleep(1000); 

        // The default state for this checkbox is checked, so clicking it should uncheck it
        bool isChecked = checkInput.Selected;
        Assert.That(isChecked, Is.False, "Checkbox should be unchecked after clicking.");

        // ==========================
        // 2. SEND KEYS
        // ==========================
        Console.WriteLine("Testing Send Keys...");
        // Clear field to empty it from any previous data
        IWebElement emailInput = _driver.FindElement(By.Name("email_input"));
        emailInput.Clear();
        Thread.Sleep(1000);

        // Enter Text
        string email = "admin@localhost.dev";
        emailInput.SendKeys(email);
        Thread.Sleep(1000);
        
        // Verify text was entered
        string data = emailInput.GetAttribute("value");
        Assert.That(data, Is.EqualTo(email), "Email input value should match the sent keys.");

        // ==========================
        // 3. CLEAR
        // ==========================
        Console.WriteLine("Testing Clear...");
        // Clear field to empty it from any previous data
        emailInput.Clear();
        Thread.Sleep(1000);
        data = emailInput.GetAttribute("value");
        
        // Verify text was cleared
        Assert.That(data, Is.EqualTo(string.Empty), "Email input value should be empty after clearing.");
        
        Console.WriteLine("Element Interactions Test Passed!");
    }

    private string GetEnvVar(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(value))
        {
            throw new Exception($"Environment variable '{key}' is missing in the .env file.");
        }
        return value;
    }
}
