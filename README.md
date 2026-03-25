# ERP Project and Selenium Automation Suite

## Overview
This project is an Enterprise Resource Planning (ERP) application focused on product management. It features a robust web interface for managing inventory, including various CRUD (Create, Read, Update, Delete) operations and data validation.

The project includes a comprehensive Selenium-based testing suite designed to ensure the reliability and stability of the user interface.

## Prerequisites
To run the project and the associated tests, you will need:
*   .NET 8.0 SDK
*   Google Chrome Browser
*   Visual Studio 2022 or JetBrains Rider (optional, but recommended)

## Technology Stack
*   **Web Framework:** ASP.NET Core 8.0 MVC
*   **Test Runner:** NUnit (v3.13.3)
*   **Automation Framework:** Selenium WebDriver (v4.41.0)
*   **Test SDK:** Microsoft.NET.Test.Sdk
*   **Browser Driver Management:** Selenium Manager (Integrated in v4+)

## Test Implementation Highlights
The automation suite follows standard industry practices to ensure reliable and scalable browser automation.

### 1. Browser Driving Mechanism (IWebDriver)
The project utilizes the `IWebDriver` interface to control the browser. This abstraction allows for seamless interaction with the DOM (Document Object Model) just as a real user would.
*   **ChromeDriver:** We specifically use `ChromeDriver` tailored for the Chrome browser.
*   **ChromeOptions:** Configured with `PageLoadStrategy.Eager` to optimize test execution speed by interacting with elements as soon as they are available in the DOM, without waiting for all sub-resources (like heavy images) to load.

### 2. Element Identification (Locators)
The tests use multiple locator strategies to interact with the UI:
*   **By.Id:** The preferred method for high-performance selection of unique elements (e.g., `#productName`).
*   **By.ClassName & By.Name:** Used for groups of elements or form-specific fields.
*   **By.XPath:** Utilized for advanced traversal, such as finding buttons by their text content (`Edit`, `Delete`).

### 3. Synchronization and Waiting Strategies
The suite employs two types of waits to handle the asynchronous nature of modern web applications (AJAX, modals, transitions):
*   **Implicit Wait:** A "global" timeout set once per driver instance. If an element isn't found immediately, the driver polls the DOM for the specified duration (e.g., 500ms) before throwing an error.
*   **Explicit Wait (WebDriverWait):** A more surgical approach used for specific elements. It waits for a specific condition (e.g., "Element is Displayed") to be met before proceeding. This is crucial for handling modals and dynamic UI updates without using fragile `Thread.Sleep()`.

### 4. Session Lifecycle Management
*   **[SetUp]:** Initializes a fresh browser instance before each test to ensure a clean state.
*   **[TearDown]:** Ensures `_driver.Quit()` is always called, which kills the browser process and frees up system resources, preventing memory leaks on the test execution machine.

### 5. Test Assertion Engine
*   **NUnit Assertions:** Used to verify application state, such as `Assert.That(myProductCard, Is.Not.Null)` and `Assert.That(title, Is.EqualTo(...))`.

## Selenium Grid Infrastructure
The project is now configured to support distributed test execution via **Selenium Grid**. This provides several advantages when sharing the project with a CTO or dev team:

*   **Remote Execution:** Instead of driving a local browser on your developer machine, the tests connect to a host at `http://localhost:4444`.
*   **Scalability:** Multiple browser nodes can be attached to the Grid, allowing for future parallel execution.
*   **Infrastructure Isolation:** It demonstrates that the testing suite is "Cloud Ready" and can be easily integrated into a CI/CD pipeline (e.g., Azure DevOps, Jenkins, or GitHub Actions).

### Selenium Manager (Integrated)
We still leverage Selenium Manager (built-in to v4), which ensures that whichever machine is hosting the Grid standalone server will have the correct browser drivers discovered and downloaded automatically.

## Test Suite Details
The `SeleniumTests` project is organized into meaningful test suites for better maintenance and clarity:

### 1. Product Lifecycle Tests
*   **File:** `ProductManagementTests.cs` (formerly `ProductCrudTests.cs`)
*   **Purpose:** Validates the complete lifecycle of a product within the ERP system.
*   **Key Checks:** Creating new products via modals, reading dashboard entries, updating product data, and verifying deletion persistence.

### 2. UI Interaction and Validation Tests
*   **File:** `ElementInteractionTests.cs`
*   **Purpose:** Tests underlying Selenium input mechanisms such as clicking, clearing, and sending keys.
*   **Key Checks:** Form control responsiveness and basic web element interaction.

### 3. Connectivity and Dashboard Smoke Tests
*   **File:** `DashboardConnectivityTests.cs`
*   **Purpose:** Ensures the application is reachable and the primary navigation elements are functioning.
*   **Key Checks:** Page titles, header verification, and modal trigger validation.

## Visual Demonstration Mode
To facilitate easy review during screen recordings for stakeholders (e.g., CTO), strategic `Thread.Sleep()` calls have been added between major UI actions. This intentionally slows down the automation so that transitions, modal openings, and data entries are clearly visible to the human eye.

## How to Run the Tests

### Cloud-Ready Configuration (Advanced)
If you move the application to a remote server (e.g., Azure or AWS), you don't need to change the source code. The suite is configured to read the application's URL from an environment variable:

*   **Variable Name:** `ERP_URL`
*   **Default:** `http://localhost:5080/Home/`

To run against a remote environment, set the variable before executing the tests:
```bash
export ERP_URL="http://your-remote-server.com/Home/"
dotnet test SeleniumTests/erp_1.SeleniumTests.csproj
```

### Step 1: Start the ERP Application
Before running the UI tests, the application must be running locally.
1.  Navigate to the project root directory.
2.  Run the application using the following command:
    ```bash
    dotnet run
    ```
3.  Ensure the application is accessible at `http://localhost:5080`.

### Step 2: Execute the Selenium Tests
1.  Open a new terminal session.
2.  Navigate to the `SeleniumTests` directory:
    ```bash
    cd SeleniumTests
    ```
3.  Execute the tests using the .NET CLI:
    ```bash
    dotnet test
    ```
Alternatively, you can run the tests directly from the root directory:
```bash
dotnet test SeleniumTests/erp_1.SeleniumTests.csproj
```

## Reviewing Test Results
After execution, the terminal will provide a summary of passed and failed tests. Detailed logs, including console output from the tests (e.g., "Testing CREATE...", "CRUD Lifecycle Test Passed!"), can be used for debugging and verification of specific steps.
