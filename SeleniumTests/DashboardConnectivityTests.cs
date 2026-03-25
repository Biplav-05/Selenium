using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace erp_1.SeleniumTests;

public class DashboardConnectivityTests
{
    [Test]
    public void ShouldVerifyDashboardConnectivity()
    {
        Console.WriteLine("Starting Dashboard Connectivity Test...");
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
        IWebDriver driver = new ChromeDriver(chromeOptions);
        driver.Manage().Window.Maximize();
        Console.WriteLine("Browser window maximized.");

        try
        {
            // 1. Navigate to your app
            Console.WriteLine("Navigating to App: http://localhost:5080/Home/");
            driver.Navigate().GoToUrl("http://localhost:5080/Home/");
            Thread.Sleep(2000); // Wait to visualize the home page

            // 2. Request browser information
            var title = driver.Title;
            Console.WriteLine("Page Title: " + title);
            Assert.That(title, Is.EqualTo("Product Management - ERP Dashboard"));

            // 3. Establish Waiting Strategy
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // 4. Find an element
            var heading = driver.FindElement(By.ClassName("gradient-text"));
            var addButton = driver.FindElement(By.ClassName("btn-primary"));
            
            // 5. Take action on element
            Console.WriteLine("Clicking 'Add Product' button...");
            addButton.Click(); 
            Thread.Sleep(2000); // Wait to see the modal pop up

            // 6. Request element information - wait for modal to be visible
            var modalTitle = wait.Until(d =>
            {
                var el = d.FindElement(By.Id("modalTitle"));
                return el.Displayed && el.Text.Length > 0 ? el : null;
            });

            Console.WriteLine("Assertion: Modal Title = " + modalTitle!.Text);
            Assert.That(modalTitle.Text, Is.EqualTo("Add New Product"));

            Console.WriteLine("Assertion: Heading = " + heading.Text);
            Assert.That(heading.Text, Is.EqualTo("Product Inventory"));

            // For learning/debugging: wait so you can see the screen
            Console.WriteLine("Waiting 2 seconds for visual verification...");
            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred: " + ex.Message);
            throw;
        }
        finally
        {
            // 7. End the session
            Console.WriteLine("Closing Driver.");
            driver.Quit();
        }
    }
}



