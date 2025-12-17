# velocityboard-Md._Shiafur_Rahman
A mini management system for projects and tasks - iTech Velocity Full-Stack .NET Task.

# VelocityBoard

A modern, responsive task management system built with ASP.NET Core 8. This application allows users to create, assign, track, and complete tasks within a project-based structure.

# Project Objectives

This project was completed as a task for iTech Velocity to demonstrate strong skills in:

* ASP.NET Core 8.0 (Razor Pages & Web API)

* Entity Framework Core (Code-First)

* JWT Authentication

* Clean Architecture (Core, Application, Infrastructure, API, Web)

* Responsive Web Design with Bootstrap 5


# Final Product

The application includes:

* Landing Page: A modern, responsive landing page with a call-to-action.
* User Authentication: Secure user registration and login system with JWT tokens.
* Dashboard: A personalized dashboard showing a user's projects and tasks.
* Project Management: Full CRUD (Create, Read, Update, Delete) functionality for projects.
* Task Management: Full CRUD functionality for tasks, including inline status updates.
* Responsive Design: The UI is clean and works seamlessly on desktop and mobile devices.

# Tech Stack
* Backend: ASP.NET Core 8.0 Web API
* Frontend: ASP.NET Core 8.0 Razor Pages
* Database: SQL Server LocalDB with Entity Framework Core
* Authentication: JSON Web Tokens (JWT)
* UI Framework: Bootstrap 5
* Architecture: Clean Architecture with Dependency Injection

# Project Structure
* VelocityBoard.Core: Contains domain models and entities (User, Project, TaskItem).
* VelocityBoard.Infrastructure: Handles data access with Entity Framework Core.
* VelocityBoard.Application: Contains business logic, services, DTOs, and AutoMapper profiles.
* VelocityBoard.API: The Web API backend providing endpoints for the frontend.
* VelocityBoard.Web: The Razor Pages frontend that consumes the API.

# How to Run the Project
* NET 8.0 SDK
* SQL Server LocalDB (or a configured SQL Server instance)

1. Clone the Repository
  * Clone the project from GitHub to your local machine.
  * git clone https://github.com/shiafrahman/velocityboard-Md._Shiafur_Rahman.git
2. Configure the Database Connection String
  * Open the VelocityBoard.API project.
  * Navigate to appsettings.json.
  * Update the DefaultConnection string.
3. Apply Database Migrations
  * Open a packager manager console (VelocityBoard.Infrastructure as Default and VelocityBoard.API as Startup Project).
4. Run the Application
  * In Solution Explorer, right-click the Solution ('Solution 'VelocityBoard') and select Set Startup Projects....
  * Select "Multiple startup projects".
  * Set the Action for both VelocityBoard.API and VelocityBoard.Web to Start.
  * Click OK.

# Default Login Credentials
The database is seeded with a few sample users for testing:
* Username: admin, john.doe, jane.smith	
* Password: Password123!(For all users)

You can create a new user by clicking register and asign Task and Project.



