# 3s-Software
This is a mock project for an interview

## How to Run

1.  **Clone the repository**
2.  **Configure `appsettings.json`**

    Create an `appsettings.json` file in the root of the project and paste the following content:

    ```json
    {
      "connectionstrings": {
        "defaultconnection": "Host=pg-1ab1b09e-ainulhasan1999-7428.c.aivencloud.com;Database=defaultdb;Username=avnadmin;Password=AVNS_sHdlxu7ayC-KxS9eMPO;Port=15125;SslMode=Require"
      },
      "Jwt": {
        "Key": "your-super-secret-jwt-key-that-is-at-least-32-characters-long-for-security",
        "Issuer": "https://localhost:7062",
        "Audience": "https://localhost:7062"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*"
    }
    ```

3.  **Run the project**

    Open a terminal in the project root and run the following command:

    ```bash
    dotnet run
    ```