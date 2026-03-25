using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace erp_1.SeleniumTests;

public class ProductManagementTests
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    private const string BaseUrl = "http://localhost:5080/Home/";

    [SetUp]
    public void Setup()
    {
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;

        _driver = new ChromeDriver(chromeOptions);
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
    }

    [TearDown]
    public void Teardown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    [Test]
    public void ShouldVerifyCompleteProductLifecycle()
    {
        _driver.Navigate().GoToUrl(BaseUrl);
        Thread.Sleep(2000); // Initial load visibility

        // ==========================
        // 1. CREATE PRODUCT
        // ==========================
        Console.WriteLine("Testing CREATE...");
        var addButton = _driver.FindElement(By.ClassName("btn-primary"));
        addButton.Click();
        Thread.Sleep(1000); // Visualizing modal open
        
        _wait.Until(d => {
            var el = d.FindElement(By.Id("modalTitle"));
            return el.Displayed ? el : null;
        });

        string testProductName = "Test Laptop " + Guid.NewGuid().ToString().Substring(0, 5);
        _driver.FindElement(By.Id("productName")).SendKeys(testProductName);
        Thread.Sleep(500); 
        _driver.FindElement(By.Id("productPrice")).SendKeys("1500.50");
        Thread.Sleep(1000); 
        
        _driver.FindElement(By.Id("saveBtn")).Click();
        Thread.Sleep(1500); // Visualizing list update

        // Wait for modal to close
        _wait.Until(d => !d.FindElement(By.Id("productModal")).GetAttribute("class").Contains("active"));

        // ==========================
        // 2. READ PRODUCT
        // ==========================
        Console.WriteLine("Testing READ...");
        var productCards = _wait.Until(d => {
            try
            {
                var cards = d.FindElements(By.ClassName("product-card"));
                return cards.Count > 0 ? cards : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        });

        var myProductCard = productCards.FirstOrDefault(c => c.Text.Contains(testProductName));
        Assert.That(myProductCard, Is.Not.Null, "Created product should be visible in the grid.");
        Thread.Sleep(1000); 

        // ==========================
        // 3. UPDATE PRODUCT
        // ==========================
        Console.WriteLine("Testing UPDATE...");
        var editButton = myProductCard.FindElement(By.XPath(".//button[contains(text(), 'Edit')]"));
        editButton.Click();
        Thread.Sleep(1000); 

        _wait.Until(d => {
            var el = d.FindElement(By.Id("modalTitle"));
            return el.Displayed && el.Text == "Edit Product" ? el : null;
        });

        var nameInput = _driver.FindElement(By.Id("productName"));
        nameInput.Clear();
        string updatedProductName = testProductName + " (Updated)";
        nameInput.SendKeys(updatedProductName);
        Thread.Sleep(500);
        
        var priceInput = _driver.FindElement(By.Id("productPrice"));
        priceInput.Clear();
        priceInput.SendKeys("1999.99");
        Thread.Sleep(1000);

        _driver.FindElement(By.Id("saveBtn")).Click();
        Thread.Sleep(1500);

        // Wait for modal to close
        _wait.Until(d => !d.FindElement(By.Id("productModal")).GetAttribute("class").Contains("active"));

        // ==========================
        // 4. DELETE PRODUCT
        // ==========================
        Console.WriteLine("Testing DELETE...");
        productCards = _driver.FindElements(By.ClassName("product-card"));
        myProductCard = productCards.FirstOrDefault(c => c.Text.Contains(updatedProductName));
        
        var deleteButton = myProductCard.FindElement(By.XPath(".//button[contains(text(), 'Delete')]"));
        deleteButton.Click();
        Thread.Sleep(1000);

        // Handle JS confirm alert
        var alert = _wait.Until(d => d.SwitchTo().Alert());
        alert.Accept();
        Thread.Sleep(1500);

        // Verify deleted
        _wait.Until(d => {
            try
            {
                var cards = d.FindElements(By.ClassName("product-card"));
                return !cards.Any(c => c.Text.Contains(updatedProductName));
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });

        Console.WriteLine("CRUD Lifecycle Test Passed!");
    }

    [Test]
    public void ShouldFailOnInvalidPrice()
    {
        Console.WriteLine("Testing UI validation for invalid price...");
        _driver.Navigate().GoToUrl(BaseUrl);
        Thread.Sleep(1000);

        var addButton = _driver.FindElement(By.ClassName("btn-primary"));
        addButton.Click();
        Thread.Sleep(1000);
        
        _driver.FindElement(By.Id("productName")).SendKeys("Invalid Price Product");
        _driver.FindElement(By.Id("productPrice")).SendKeys("invalid_string");
        Thread.Sleep(1000);
        
        _driver.FindElement(By.Id("saveBtn")).Click();
        Thread.Sleep(2000); // Visibility for the validation failure

        bool isModalActive = _driver.FindElement(By.Id("productModal")).GetAttribute("class").Contains("active");
        Assert.That(isModalActive, Is.True, "Modal should remain active due to invalid price.");
    }
}
