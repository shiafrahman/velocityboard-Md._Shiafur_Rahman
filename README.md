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
  * git clone https://github.com/YourUsername/velocityboard-YourName.gitcd velocityboard-YourName
