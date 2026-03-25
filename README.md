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

## Selenium Grid and Docker Infrastructure
To ensure distributed test execution and standard environments, we utilize **Selenium Grid** along with **Docker Compose**. This setup provides several advantages:

*   **Docker Hub/Node Setup**: Instead of maintaining a local driver installation, we run a **Selenium Hub** and a **Chrome Node** in containers.
*   **Browser Isolation**: The Chrome browser runs inside Docker, meaning no Chrome windows pop up and no system configurations are changed on your local OS.
*   **Scalability**: You can easily scale the grid to multiple nodes for parallel execution.
*   **Visual Debugging**: Use Port **7900** for NoVNC (Password: `secret`) to watch the tests live at [http://localhost:7900](http://localhost:7900).

### Environment Configuration
The project is configured to respond to environment variables defined in a `.env` file. This allows for seamless transitions between local and Docker environments.
*   Check out the **[Configuration Guide (config.md)](config.md)** for more details on each variable.


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
To facilitate easy review during screen recordings, strategic `Thread.Sleep()` calls have been added between major UI actions. This intentionally slows down the automation so that transitions, modal openings, and data entries are clearly visible to the human eye.

## How to Run the Project

### Step 1: Set up the Configuration
1.  Verify the **[.env](.env)** file exists in the root directory.
2.  Set `IS_LOCAL_SETUP=false` to use the Docker Grid (Recommended).
3.  Set `IS_LOCAL_SETUP=true` to use your computer's local Chrome browser.

### Step 2: Start the Selenium Infrastructure (Docker Only)
If you chose the Docker mode (Step 1), start the Grid:
```bash
docker-compose up -d
```

### Step 3: Run the ERP Application
Ensure the application is running locally:
```bash
dotnet run
```
*The app is configured in `launchSettings.json` to listen on `0.0.0.0:5080` so Docker can reach it.*

### Step 4: Execute the Selenium Tests
Execute the test suite using the .NET CLI:
```bash
dotnet test SeleniumTests/erp_1.SeleniumTests.csproj
```

### Step 5: (Optional) Visualize Docker Testing
If running in Docker, you can watch the browser live:
1.  Open Chrome/Firefox and go to: [http://localhost:7900](http://localhost:7900)
2.  Click **Connect** and enter password: `secret`

---

## Reviewing Test Results
After execution, the terminal will provide a summary of the test run. Detailed console logs (e.g., "Testing CREATE...", "CRUD Lifecycle Test Passed!") are produced by each test for easier debugging and stakeholders review.


## Reviewing Test Results
After execution, the terminal will provide a summary of passed and failed tests. Detailed logs, including console output from the tests (e.g., "Testing CREATE...", "CRUD Lifecycle Test Passed!"), can be used for debugging and verification of specific steps.
