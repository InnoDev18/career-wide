# Career-Wide

Career-Wide is a job listing and application web app built with ASP.NET Core MVC. Administrators can manage job listings, and visitors can browse open roles and submit applications.

> **Project status: Work in progress. I am currently working on this project, and features and design may continue to change.**

## Current features

- Public page for browsing published job listings that are still open.
- Admin sign-in with role-protected job management.
- Create, view, edit, publish, and manually delete job listings.
- Automatic posting date and an employment-type dropdown when managing listings.
- Online job applications with applicant details and a cover letter.
- Admin page for reviewing submitted applications.
- Responsive interface with a dark and red theme.

## Built with

- ASP.NET Core MVC on .NET 10
- C# and Razor views
- Entity Framework Core 10
- SQL Server Express LocalDB for local development
- ASP.NET Core Identity
- Bootstrap and CSS

## Run locally

### Requirements

- Visual Studio 2026 with the .NET 10 SDK and ASP.NET and web development workload
- SQL Server Express LocalDB

### Setup

1. Clone the repository and open `career-wide.slnx` in Visual Studio.
2. In Visual Studio, set the `career-wide` project as the startup project.
3. Check the `DefaultConnection` value in `career-wide/appsettings.json`. The checked-in value uses `(localdb)\MSSQLLocalDB` for local development.
4. Apply the database migrations using **Tools → NuGet Package Manager → Package Manager Console**, then run:

   ```powershell
   Update-Database
   ```

5. To create the initial administrator account, right-click the `career-wide` project in Solution Explorer, choose **Manage User Secrets**, and add your own local values:

   ```json
   {
     "AdminUser": {
       "Email": "your-email@example.com",
       "Password": "choose-a-strong-local-password"
     }
   }
   ```

   Keep these credentials in User Secrets. Do not commit real passwords to the repository.

6. Run the project from Visual Studio.

## Notes

- The included LocalDB connection string is intended for local development. A different connection string is needed for another environment.
- The administrator account is created from the `AdminUser` values in User Secrets when the application starts.
- This project is under active development and is not presented as a production deployment.

