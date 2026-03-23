using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace erp_1.SeleniumTests;

public class FirstScript
{
    [Test]
    public void EightComponents()
    {
        Console.WriteLine("Starting Selenium Test...");
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        Console.WriteLine("Browser window maximized.");

        try
        {
            // 2. Navigate to your app
            Console.WriteLine("Navigating to App: http://localhost:5080/Home/");
            driver.Navigate().GoToUrl("http://localhost:5080/Home/");

            // 3. Request browser information
            var title = driver.Title;
            Console.WriteLine("Page Title: " + title);
            Assert.That(title, Is.EqualTo("Product Management - ERP Dashboard"));

            // 4. Establish Waiting Strategy
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // 5. Find an element
            var heading = driver.FindElement(By.ClassName("gradient-text"));
            var addButton = driver.FindElement(By.ClassName("btn-primary"));
            
            // 6. Take action on element
            Console.WriteLine("Clicking 'Add Product' button...");
            addButton.Click(); 

            // 7. Request element information - wait for modal to be visible
            var modalTitle = wait.Until(d =>
            {
                var el = d.FindElement(By.Id("modalTitle"));
                return el.Displayed && el.Text.Length > 0 ? el : null;
            });
            Thread.Sleep(5000);

            Console.WriteLine("Assertion: Modal Title = " + modalTitle!.Text);
            Assert.That(modalTitle.Text, Is.EqualTo("Add Old Product"));

            Console.WriteLine("Assertion: Heading = " + heading.Text);
            Assert.That(heading.Text, Is.EqualTo("Product Inventory"));

            // For learning/debugging: wait so you can see the screen
            Console.WriteLine("Waiting 5 seconds for visual verification...");
            Thread.Sleep(5000);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred: " + ex.Message);
            throw;
        }
        finally
        {
            // 8. End the session
            Console.WriteLine("Closing Driver.");
            driver.Quit();
        }
    }
}



