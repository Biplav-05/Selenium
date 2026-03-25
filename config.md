# Environment Configuration Guide

This project uses a `.env` file to manage all Selenium and Application configurations. This allows the test suite to be portable across different environments (local dev, Docker Grid, CI/CD) without changing the source code.

## The `.env` File
The `.env` file should be located in the **project root directory**.

### Available Variables

| Variable Name | Description | Recommended Value (Local) | Recommended Value (Docker) |
| :--- | :--- | :--- | :--- |
| `IS_LOCAL_SETUP` | Toggle between local browser and Docker Grid. | `true` | `false` |
| `SEL_GRID_HUB_URL` | The URL of the Selenium Hub. | `http://localhost:4444` | `http://localhost:4444` |
| `APP_URL_LOCAL` | App URL when running tests on your host. | `http://localhost:5080/Home/` | `N/A` |
| `APP_URL_DOCKER` | App URL when running browser in Docker. | `N/A` | `http://host.docker.internal:5080/Home/` |

---

## 1. Local Testing Mode (`IS_LOCAL_SETUP=true`)
In this mode, Selenium will open a Chrome window directly on your physical desktop.
*   **Requirements**: Google Chrome installed on your machine.
*   **Networking**: Uses `localhost` to reach the ERP app.

## 2. Docker Grid Mode (`IS_LOCAL_SETUP=false`)
In this mode, Selenium runs inside a Docker container. You won't see a browser window pop up, but you can "remote in" via a web browser.
*   **Requirements**: Docker and Docker Compose installed.
*   **Networking**: Uses `host.docker.internal` to reach the ERP app running on your host machine.
*   **Visualizing**: Open [http://localhost:7900](http://localhost:7900) (Password: `secret`) to watch the tests.

---

## Troubleshooting Connectivity
If you see a `CONNECTION_REFUSED` error during tests:
1.  Ensure your ERP app is running on port `5080`.
2.  Ensure your ERP app is listening on `0.0.0.0` (not just `localhost`). We have updated `launchSettings.json` to handle this.
3.  Check that `IS_LOCAL_SETUP` matches your current infrastructure choice.
