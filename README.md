# 🎉 Event Management Application

Welcome to the Event Management Application! This application is built with ASP.NET Core to help you manage events efficiently.

## 📋 Table of Contents

- Setting Up `appsettings.json`
- Important Note
- Running the Application
- Creating and Applying Migrations
- Troubleshooting

## 🛠️ Setting Up `appsettings.json`

Create a file named `appsettings.json` in the root directory of your project and add the following configuration:

```json
{
    "JWT": {
    "Key": "qwertyuiopasdfghjklzsssxcvbnm123456",
    "Issuer": "https://mysite.io",
    "Audience": "https://app.mysite.io"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "Server=<Add Your Connection String>;Database=EventManageCFA;Trusted_Connection=Yes;MultipleActiveResultSets=true;TrustServerCertificate=true"
    }
}
```
## ⚠️ Important Note

To ensure that your `appsettings.json` file, which contains sensitive information like your connection string, is not included in your Git repository, add it to your `.gitignore` file:

appsettings.json
appsettings.Development.json
## 🚀 Running the Application

1. **Clone the repository**:
   ```sh
   git clone https://github.com/ShyamLal-P/Event-Management-System.git
   ```
Navigate to the project directory:

```sh
cd EventManagementTrialCFA
```
Restore the dependencies:

```sh
dotnet restore
```
Apply the migrations to update the database:

```sh
dotnet ef database update
```
Run the application:

```sh
dotnet run
```
🛠️ Creating and Applying Migrations
Add a new migration:

```sh
dotnet ef migrations add <MigrationName>
```
Apply the migration to the database:

```sh
dotnet ef database update
```
## 🌟 Features
1. **Event Management**: Create, update, and manage events with ease.
2. **Ticket Booking**: Users can book tickets for events, with status tracking.
3. **Feedback Submission**: Users can submit feedback for events they have attended, with validation to ensure feedback is only submitted for booked tickets.
4. **Average Rating Calculation**: Calculate and retrieve the average rating for each event.
5. **Ticket Cancellation**: Users can cancel their booked tickets.
6. **User Authentication**: Secure user authentication and authorization using ASP.NET Identity.

## 🛠️ Troubleshooting

### Database Connection Issues
Ensure that your connection string in `appsettings.json` is correct and that your SQL Server instance is running.

### Migration Errors
If you encounter errors during migration, you may need to revert the migration and try again. Use the following commands:
```sh
dotnet ef migrations remove
```
```sh
dotnet ef migrations add <MigrationName>
```
```sh
dotnet ef database update
```
Feel free to reach out if you have any questions or need further assistance!

Happy coding! 🎉
